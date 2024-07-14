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

            if (taskDto == null || taskDto.Id == Guid.Empty)
            {
                // Something went wrong
                return BadRequest();
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
            var phaseDto = await PhasesController.CreatePhases(addPhaseRequestDto);
            if (phaseDto is OkObjectResult okPhaseResult && okPhaseResult.Value != null)
                taskDto.Phases = new List<PhaseDto>((List<PhaseDto>)okPhaseResult.Value);

            // Role
            AddRoleRequestDto addRoleRequestDto = new AddRoleRequestDto
            {
                RoleType = RoleType.Editor,
                TaskId = taskDto.Id,
                ProfileId = addTaskRequestDto.ProfileId // TODO: Use profile id when profile is ready
            };
            var roleDto = await rolesController.Create(addRoleRequestDto);
            if (roleDto is OkObjectResult okRoleResult && okRoleResult.Value != null)
                taskDto.Roles = new List<RoleDto> { (RoleDto)okRoleResult.Value };

            // Invitation
            addTaskRequestDto.Invitation.TaskId = taskDto.Id;
            addTaskRequestDto.Invitation.RoleType = RoleType.Tracker;
            var invitationDto = await invitationsController.Create(addTaskRequestDto.Invitation);
            if (invitationDto is OkObjectResult okInvitationResult && okInvitationResult.Value != null)
                taskDto.Invitation = (InvitationDto)okInvitationResult.Value;

            return Ok(taskDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var taskDomainModel = await taskRepository.GetByIdAsync(id);

            if (taskDomainModel != null)
            {
                var taskDto = mapper.Map<TaskDto>(taskDomainModel);

                // Get Phases by task id
                var phaseDto = await PhasesController.GetAllByTaskId(id);
                if (phaseDto is OkObjectResult okPhaseResult && okPhaseResult.Value != null)
                    taskDto.Phases = new List<PhaseDto>((List<PhaseDto>)okPhaseResult.Value);

                // Get Role by task id
                var roleDto = await rolesController.GetAllByTaskId(id);
                if (roleDto is OkObjectResult okRoleResult && okRoleResult.Value != null)
                    taskDto.Roles = new List<RoleDto>((List<RoleDto>)okRoleResult.Value);

                // Get Invitation by task id
                var invitationDto = await invitationsController.GetByTaskId(id);
                if (invitationDto is OkObjectResult okInvitationResult && okInvitationResult.Value != null)
                    taskDto.Invitation = (InvitationDto)okInvitationResult.Value;

                return Ok(taskDto);
            }
            else
            {
                // Something went wrong
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("ByProfileId/{id:Guid}")]
        public async Task<IActionResult> GetAllByProfileId([FromRoute] Guid id)
        {
            var rolesDto = await rolesController.GetAllByProfileId(id);

            if (rolesDto is OkObjectResult okRolesResult && okRolesResult.Value != null)
            {
                List<RoleDto> roles = (List<RoleDto>)okRolesResult.Value;
                if (roles.Count > 0)
                {
                    var tasks = new List<TaskDto>();
                    foreach (var role in roles)
                    {
                        var taskId = role.TaskId;
                        var taskDomainModel = await taskRepository.GetByIdAsync(taskId);

                        if (taskDomainModel != null)
                        {
                            var taskDto = mapper.Map<TaskDto>(taskDomainModel);

                            // Get Phases by task id
                            var phaseDto = await PhasesController.GetAllByTaskId(taskId);
                            if (phaseDto is OkObjectResult okPhaseResult && okPhaseResult.Value != null)
                                taskDto.Phases = new List<PhaseDto>((List<PhaseDto>)okPhaseResult.Value);

                            // Get Invitation by task id
                            var invitationDto = await invitationsController.GetByTaskId(taskId);
                            if (invitationDto is OkObjectResult okInvitationResult && okInvitationResult.Value != null)
                                taskDto.Invitation = (InvitationDto)okInvitationResult.Value;

                            taskDto.Roles = roles.Where(r => r.TaskId == taskDto.Id).ToList();

                            tasks.Add(taskDto);
                        }
                    }
                    return Ok(tasks);
                }
                return Ok(new List<TaskDto>());
            }
            else
            {
                // Something went wrong
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedTaskDomainModel = await taskRepository.DeleteAsync(id);

            if (deletedTaskDomainModel == null)
                return NotFound();

            var deletedTaskDto = mapper.Map<TaskDto>(deletedTaskDomainModel);

            return Ok(deletedTaskDto);
        }
    }
}
