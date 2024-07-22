using AutoMapper;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInvitationRepository invitationRepository;
        private readonly ITaskRepository taskRepository;

        public InvitationsController(IMapper mapper, IInvitationRepository invitationRepository, ITaskRepository taskRepository)
        {
            this.mapper = mapper;
            this.invitationRepository = invitationRepository;
            this.taskRepository = taskRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddInvitationRequestDto addInvitationRequestDto)
        {
            if(addInvitationRequestDto.TaskId.Equals(Guid.Empty))
                return BadRequest("TaskId is required");

            if (addInvitationRequestDto.Task != null && addInvitationRequestDto.Task.Status == Enums.TaskStatus.Rejected)
            {
                var taskDomainModel = mapper.Map<Model.Domain.Task>(addInvitationRequestDto.Task);
                taskDomainModel.Status = Enums.TaskStatus.Pending;
                await taskRepository.UpdateAsync(addInvitationRequestDto.TaskId, taskDomainModel);
            }

            addInvitationRequestDto.Task = null;

            var invitationDomainModel = mapper.Map<Invitation>(addInvitationRequestDto);

            await invitationRepository.CreateAsync(invitationDomainModel);

            var invitationDto = mapper.Map<InvitationDto>(invitationDomainModel);

            return Ok(invitationDto);
        }

        [HttpGet]
        [Route("ByTaskId/{taskId:Guid}")]
        public async Task<IActionResult> GetByTaskId([FromRoute] Guid taskId)
        {
            var invitationDomainModel = await invitationRepository.GetByTaskIdAsync(taskId);

            var invitationDto = mapper.Map<InvitationDto>(invitationDomainModel);

            return Ok(invitationDto);
        }

        [HttpGet]
        [Route("AllByPhoneNumber/{phoneNumber}")]
        public async Task<IActionResult> GetAllByPhoneNumber([FromRoute] string phoneNumber)
        {
            var invitationsDomainModel = await invitationRepository.GetAllByPhoneNumberAsync(phoneNumber);

            var invitationsDto = mapper.Map<List<InvitationDto>>(invitationsDomainModel);

            return Ok(invitationsDto);
        }
        [HttpGet]
        [Route("AllByTaskId/{taskId:Guid}")]
        public async Task<IActionResult> GetAllByTask([FromRoute] Guid taskId)
        {
            var invitationsDomainModel = await invitationRepository.GetAllByTaskAsync(taskId);

            var invitationsDto = mapper.Map<List<InvitationDto>>(invitationsDomainModel);

            return Ok(invitationsDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedInvitationDomainModel = await invitationRepository.DeleteAsync(id);

            if(deletedInvitationDomainModel == null)
                return NotFound();

            var deletedInvitationDto = mapper.Map<InvitationDto>(deletedInvitationDomainModel);

            return Ok(deletedInvitationDto);
        }
    }
}
