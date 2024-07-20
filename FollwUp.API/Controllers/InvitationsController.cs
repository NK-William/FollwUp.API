using AutoMapper;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInvitationRepository invitationRepository;

        public InvitationsController(IMapper mapper, IInvitationRepository invitationRepository)
        {
            this.mapper = mapper;
            this.invitationRepository = invitationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddInvitationRequestDto addInvitationRequestDto)
        {
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
