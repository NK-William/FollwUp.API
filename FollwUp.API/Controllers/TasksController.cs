using AutoMapper;
using FollwUp.API.Enums;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain = FollwUp.API.Model.Domain;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITaskRepository taskRepository;
        private readonly PhasesController PhasesController;
        private readonly RolesController rolesController;
        private readonly InvitationsController invitationsController;

        public TasksController(IMapper mapper, ITaskRepository taskRepository, PhasesController PhasesController,
            RolesController rolesController, InvitationsController invitationsController)
        {
            this.mapper = mapper;
            this.taskRepository = taskRepository;
            this.PhasesController = PhasesController;
            this.rolesController = rolesController;
            this.invitationsController = invitationsController;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTaskRequestDto addTaskRequestDto)
        {
            var taskDomainModel = mapper.Map<Domain.Task>(addTaskRequestDto);

            await taskRepository.CreateAsync(taskDomainModel);

            var taskDto = mapper.Map<TaskDto>(taskDomainModel);

            if (taskDto == null || taskDto.Id == null)
            {
                // Something went wrong
            }

            // Phase
            List<AddPhaseRequestDto> addPhaseRequestDto = addTaskRequestDto.Phases.Select(p => new AddPhaseRequestDto
            {
                Name = p.Name,
                Description = p.Description,
                Icon = p.Icon,
                Number = p.Number,
                Status = p.Status,
                TaskId = taskDto.Id
            }).ToList();
            var phaseDto = await PhasesController.Create(addPhaseRequestDto);
            if (phaseDto is OkObjectResult okPhaseResult && okPhaseResult.Value != null)
                taskDto.Phases = new List<PhaseDto>((List<PhaseDto>)okPhaseResult.Value);

            // Role
            AddRoleRequestDto addRoleRequestDto = new AddRoleRequestDto
            {
                RoleType = RoleType.Editor,
                TaskId = taskDto.Id,
                //ProfileId = Guid.NewGuid() // TODO: Use profile id when profile is ready
            };
            var roleDto = await rolesController.Create(addRoleRequestDto);
            if (roleDto is OkObjectResult okRoleResult && okRoleResult.Value != null)
                taskDto.Role = (RoleDto)okRoleResult.Value;

            // Invitation
            addTaskRequestDto.Invitation.TaskId = taskDto.Id;
            var invitationDto = await invitationsController.Create(addTaskRequestDto.Invitation);
            if (invitationDto is OkObjectResult okInvitationResult && okInvitationResult.Value != null)
                taskDto.Invitation = (InvitationDto)okInvitationResult.Value;

            return Ok(taskDto);
        }
    }
}
