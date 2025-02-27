using AutoMapper;
using FollwUp.API.Enums;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.RegularExpressions;
using Domain = FollwUp.API.Model.Domain;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITaskRepository taskRepository;
        private readonly PhasesController phasesController;
        private readonly RolesController rolesController;
        private readonly InvitationsController invitationsController;

        public TasksController(IMapper mapper, ITaskRepository taskRepository, PhasesController phasesController,
            RolesController rolesController, InvitationsController invitationsController)
        {
            this.mapper = mapper;
            this.taskRepository = taskRepository;
            this.phasesController = phasesController;
            this.rolesController = rolesController;
            this.invitationsController = invitationsController;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTaskRequestDto addTaskRequestDto)
        {
            if(addTaskRequestDto.ProfileId.Equals(Guid.Empty))
                return BadRequest("ProfileId must be valid.");

            if (addTaskRequestDto.Eta < DateTime.Today)
                return BadRequest("ETA cannot be in the past");

            var taskDomainModel = mapper.Map<Domain.Task>(addTaskRequestDto);

            await taskRepository.CreateAsync(taskDomainModel);

            var taskDto = mapper.Map<TaskDto>(taskDomainModel);

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
            var phaseDto = await phasesController.CreatePhases(addPhaseRequestDto);
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
                // ToDo: Clean this code
                var taskDto = mapper.Map<TaskDto>(taskDomainModel);

                // Get Phases by task id
                var phaseDto = await phasesController.GetAllByTaskId(id);
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
                return NotFound();
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
                            // ToDo: Clean this code
                            var taskDto = mapper.Map<TaskDto>(taskDomainModel);

                            // Get Phases by task id
                            var phaseDto = await phasesController.GetAllByTaskId(taskId);
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
                return Ok("Failed to retrieve tasks due to roles not being defined for a given user");
            }
            return NotFound("No tasks found for the given profile id");
        }

        [HttpGet]
        [Route("GetPending/{phoneNumber}")]
        public async Task<IActionResult> GetAllByPhoneNumber([FromRoute] string phoneNumber)
        {
            var invitationsDto = await invitationsController.GetAllByPhoneNumber(phoneNumber);

            if (invitationsDto is OkObjectResult okInvitationsResult && okInvitationsResult.Value != null)
            {
                List<InvitationDto> invitations = (List<InvitationDto>)okInvitationsResult.Value;
                if (!invitations.Any())
                    return NotFound("No tasks found for the given phone number");

                var tasks = new List<TaskDto>();
                foreach (var invitation in invitations)
                {
                    var taskId = invitation.TaskId;
                    var taskDomainModel = await taskRepository.GetByIdAsync(taskId);

                    if (taskDomainModel != null)
                    {
                        // ToDo: Clean this code
                        var taskDto = mapper.Map<TaskDto>(taskDomainModel);

                        // Get Phases by task id
                        var phaseDto = await phasesController.GetAllByTaskId(taskId);
                        if (phaseDto is OkObjectResult okPhaseResult && okPhaseResult.Value != null)
                            taskDto.Phases = new List<PhaseDto>((List<PhaseDto>)okPhaseResult.Value);

                        // Get Role by task id
                        var roleDto = await rolesController.GetAllByTaskId(taskId);
                        if (roleDto is OkObjectResult okRoleResult && okRoleResult.Value != null)
                            taskDto.Roles = new List<RoleDto>((List<RoleDto>)okRoleResult.Value);

                        taskDto.Invitation = invitation;

                        tasks.Add(taskDto);
                    }
                }
                return Ok(tasks);
            }
            return BadRequest("No tasks found for the given phone number");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTaskRequestDto updateTaskRequestDto)
        {
            var taskDomainModel = mapper.Map<Domain.Task>(updateTaskRequestDto);

            taskDomainModel = await taskRepository.UpdateAsync(id, taskDomainModel);

            if (taskDomainModel == null)
                return NotFound();

            var taskDto = mapper.Map<TaskDto>(taskDomainModel);

            return Ok(taskDto);
        }

        [HttpPut]
        [Route("Accept/{id:Guid}")]
        public async Task<IActionResult> Accept([FromRoute] Guid id, [FromBody] AcceptTaskRequestDto acceptTaskRequestDto)
        {

            if (acceptTaskRequestDto.InvitationId.Equals(Guid.Empty))
                return BadRequest("Invitation must be valid");

            if(acceptTaskRequestDto.ProfileId.Equals(Guid.Empty))
                return BadRequest("ProfileId must be valid.");

            var taskDomainModel = mapper.Map<Domain.Task>(acceptTaskRequestDto.Task);

            taskDomainModel = await taskRepository.UpdateAsync(id, taskDomainModel);

            if (taskDomainModel == null)
                return NotFound();

            var taskDto = mapper.Map<TaskDto>(taskDomainModel);

            await invitationsController.Delete(acceptTaskRequestDto.InvitationId);

            // Create role
            await rolesController.Create(new AddRoleRequestDto
            {
                RoleType = acceptTaskRequestDto.RoleType,
                TaskId = id,
                ProfileId = acceptTaskRequestDto.ProfileId
            });

            // Get Phases by task id
            var phaseDto = await phasesController.GetAllByTaskId(id);
            if (phaseDto is OkObjectResult okPhaseResult && okPhaseResult.Value != null)
                taskDto.Phases = new List<PhaseDto>((List<PhaseDto>)okPhaseResult.Value);

            // Get Role by task id
            var roleDto = await rolesController.GetAllByTaskId(id);
            if (roleDto is OkObjectResult okRoleResult && okRoleResult.Value != null)
                taskDto.Roles = new List<RoleDto>((List<RoleDto>)okRoleResult.Value);

            return Ok(taskDto);
        }

        [HttpPut]
        [Route("Reject/{id:Guid}")]
        public async Task<IActionResult> Reject([FromRoute] Guid id, [FromBody] RejectTaskRequestDto rejectTaskRequestDto)
        {
            if (rejectTaskRequestDto.InvitationId.Equals(Guid.Empty))
                return BadRequest("InvitationId must be valid");

            // Rejecting invitation
            // If (no Role(s) of roleType View or Tracker -) and got (one invite +) => set Task status to rejected

            // Get Roles by task id
            var rolesDto = await rolesController.GetAllByTaskId(id);
            List<RoleDto> roles = new List<RoleDto>();
            if (rolesDto is OkObjectResult okRolesResult && okRolesResult.Value != null)
                roles = (List<RoleDto>)okRolesResult.Value;

            // Get Invitations by task id
            var invitationsDto = await invitationsController.GetAllByTask(id);
            List<InvitationDto> invitations = new List<InvitationDto>();
            if (invitationsDto is OkObjectResult okInvitationsResult && okInvitationsResult.Value != null)
                invitations = (List<InvitationDto>)okInvitationsResult.Value;

            // If no Role(s) of roleType View or Tracker
            var hasViewOrTrackerRole = roles.Exists(r => r.RoleType == RoleType.Viewer || r.RoleType == RoleType.Tracker);

            if (invitations.Count == 1 && !hasViewOrTrackerRole)
            {
                var taskDomainModel = mapper.Map<Domain.Task>(rejectTaskRequestDto.Task);
                taskDomainModel.Status = Enums.TaskStatus.Rejected;
                await taskRepository.UpdateAsync(id, taskDomainModel);
            }

            // Delete invite
            await invitationsController.Delete(rejectTaskRequestDto.InvitationId);

            return Ok();
        }

        [HttpPut]
        [Route("Complete/{id:Guid}")]
        public async Task<IActionResult> Complete([FromRoute] Guid id)
        {
            var taskDomainModel = await taskRepository.GetByIdAsync(id);

            if (taskDomainModel == null)
                return NotFound();

            // Update task status to completed
            taskDomainModel.Status = Enums.TaskStatus.Completed;

            taskDomainModel = await taskRepository.UpdateAsync(id, taskDomainModel);

            var taskDto = mapper.Map<TaskDto>(taskDomainModel);
            taskDto.Phases = new List<PhaseDto>();

            // Update all phases to completed
            var phasesDto = await phasesController.GetAllByTaskId(id);
            if (phasesDto is OkObjectResult okPhasesResult && okPhasesResult.Value != null)
            {
                List<PhaseDto> phases = (List<PhaseDto>)okPhasesResult.Value;
                foreach (var phase in phases)
                {
                    if(phase.Status != TaskPhaseStatus.Completed)
                    {
                        var updatePhaseRequestDto = mapper.Map<UpdatePhaseRequestDto>(phase);
                        updatePhaseRequestDto.Status = TaskPhaseStatus.Completed;

                        await phasesController.Update(phase.id, false, updatePhaseRequestDto);
                    }

                    taskDto.Phases.Add(phase);
                }
            }

            return Ok(taskDto);
        }


        [HttpPut]
        [Route("Disconnect/{id:Guid}")]
        public async Task<IActionResult> Disconnect([FromRoute] Guid id, DisconnectTaskRequestDto disconnectTaskRequestDto)
        {
            if (disconnectTaskRequestDto.RoleId.Equals(Guid.Empty))
                return BadRequest("Role must be valid");

            await rolesController.Delete(disconnectTaskRequestDto.RoleId);

            // Get Roles by task id
            var rolesDto = await rolesController.GetAllByTaskId(id);
            List<RoleDto> roles = new List<RoleDto>();
            if (rolesDto is OkObjectResult okRolesResult && okRolesResult.Value != null)
                roles = (List<RoleDto>)okRolesResult.Value;

            // Get Invitations by task id
            var invitationsDto = await invitationsController.GetAllByTask(id);
            List<InvitationDto> invitations = new List<InvitationDto>();
            if (invitationsDto is OkObjectResult okInvitationsResult && okInvitationsResult.Value != null)
                invitations = (List<InvitationDto>)okInvitationsResult.Value;

            // If no Role(s) of roleType View or Tracker
            var hasViewOrTrackerRole = roles.Exists(r => r.RoleType == RoleType.Viewer || r.RoleType == RoleType.Tracker);

            if (!invitations.Any() && !hasViewOrTrackerRole)
            {
                var taskDomainModel = mapper.Map<Domain.Task>(disconnectTaskRequestDto.Task);
                taskDomainModel.Status = Enums.TaskStatus.Rejected;
                await taskRepository.UpdateAsync(id, taskDomainModel);
            }

            return Ok();
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
