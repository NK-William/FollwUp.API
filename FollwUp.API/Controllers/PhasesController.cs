using AutoMapper;
using FollwUp.API.Enums;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhasesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPhaseRepository phaseRepository;
        private readonly ITaskRepository taskRepository;

        public PhasesController(IMapper mapper, IPhaseRepository phaseRepository, ITaskRepository taskRepository)
        {
            this.mapper = mapper;
            this.phaseRepository = phaseRepository;
            this.taskRepository = taskRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPhaseRequestDto addPhaseRequestDto)
        {
            if(addPhaseRequestDto.TaskId.Equals(Guid.Empty))
                return BadRequest("TaskId must be valid.");

            var phaseDomainModel = mapper.Map<Phase>(addPhaseRequestDto);

            await phaseRepository.CreateAsync(phaseDomainModel);

            var foundTaskCompleted = false;

            // TODO Code1::: This code is duplicated
            //Revert task status to pending if is currently completed
            var taskDomainModel = await taskRepository.GetByIdAsync(addPhaseRequestDto.TaskId);
            if(taskDomainModel != null)
            {
                if(taskDomainModel.Status == Enums.TaskStatus.Completed)
                {
                    foundTaskCompleted = true;
                    taskDomainModel.Status = Enums.TaskStatus.Pending;
                    await taskRepository.UpdateAsync(addPhaseRequestDto.TaskId, taskDomainModel);
                }
            }

            // get all by task id
            var phasesDomainModel = await phaseRepository.GetAllByTaskIdAsync(addPhaseRequestDto.TaskId);

            if (phaseDomainModel.Number < phasesDomainModel.Count)
            {
                // order by number
                phasesDomainModel = phasesDomainModel.OrderBy(p => p.Number).ToList();

                // update number
                for (int i = 0; i < phasesDomainModel.Count; i++)
                {
                    int currentNumber = i + 1;
                    if (phaseDomainModel.Number == currentNumber)
                    {
                        var container = phasesDomainModel[i]; // Old phase

                        // Editing new phase
                        phasesDomainModel[i] = phaseDomainModel;

                        // Keep status of old phase
                        phasesDomainModel[i].Status = container.Status;

                        // Editing old phase
                        phasesDomainModel[i + 1] = container;
                        phasesDomainModel[i + 1].Number = currentNumber + 1;

                        if(phasesDomainModel[i].Status == TaskPhaseStatus.InProgress)
                            phasesDomainModel[i + 1].Status = TaskPhaseStatus.Pending;

                        continue;
                    }

                    if (phaseDomainModel.Number == currentNumber - 1)
                        continue;

                    phasesDomainModel[i].Number = currentNumber;
                }

                // update all phases
                foreach (var phase in phasesDomainModel)
                {
                    await phaseRepository.UpdateAsync(phase.id, phase);
                }
            }else if(foundTaskCompleted){
                // We are adding the last phase when the task was completed, so we need to update the status of the last phase to In-Progress
                var lastPhase = phasesDomainModel[phasesDomainModel.Count - 1];

                lastPhase.Status = TaskPhaseStatus.InProgress;
                await phaseRepository.UpdateAsync(lastPhase.id, lastPhase);
            }

            var phaseDto = mapper.Map<PhaseDto>(phaseDomainModel);

            return Ok(phaseDto);
        }

        [HttpPost]
        [Route("List")]
        public async Task<IActionResult> CreatePhases([FromBody] List<AddPhaseRequestDto> addPhasesRequestDto)
        {
            if (addPhasesRequestDto.Exists(dto => dto.TaskId.Equals(Guid.Empty)))
            {
                return BadRequest("All items should have a valid TaskId");
            }

            var phasesDomainModel = mapper.Map<List<Phase>>(addPhasesRequestDto);

            await phaseRepository.CreatePhasesAsync(phasesDomainModel);

            var phasesDto = mapper.Map<List<PhaseDto>>(phasesDomainModel);

            return Ok(phasesDto);
        }

        [HttpGet]
        [Route("GetAllByTaskId/{id:Guid}")]
        public async Task<IActionResult> GetAllByTaskId([FromRoute] Guid id)
        {
            var phasesDomainModel = await phaseRepository.GetAllByTaskIdAsync(id);

            var phasesDto = mapper.Map<List<PhaseDto>>(phasesDomainModel);

            return Ok(phasesDto);
        }

        [HttpPut]
        [Route("{id:Guid}/")]
        public async Task<IActionResult> Update([FromRoute] Guid id, bool statusOnly, [FromBody] UpdatePhaseRequestDto updatePhaseRequestDto)
        {
            if(updatePhaseRequestDto.TaskId.Equals(Guid.Empty))
                return BadRequest("TaskId must be valid.");

            if(id.Equals(Guid.Empty))
                return BadRequest("Id must be valid.");

            var phaseDomainModel = mapper.Map<Phase>(updatePhaseRequestDto);

            await phaseRepository.UpdateAsync(id, phaseDomainModel);

            if (statusOnly && phaseDomainModel.Status == TaskPhaseStatus.InProgress)
            {
                // get all by task id
                var phasesDomainModel = await phaseRepository.GetAllByTaskIdAsync(phaseDomainModel.TaskId);

                for (int i = 0; i < phasesDomainModel.Count; i++)
                {
                    if (phasesDomainModel[i].Number < phaseDomainModel.Number)
                        phasesDomainModel[i].Status = TaskPhaseStatus.Completed;

                    if (phasesDomainModel[i].Number > phaseDomainModel.Number)
                        phasesDomainModel[i].Status = TaskPhaseStatus.Pending;
                }

                // update all phases
                foreach (var phase in phasesDomainModel)
                {
                    await phaseRepository.UpdateAsync(phase.id, phase);
                }

                // TODO Code1::: This code is duplicated
                //Revert task status to pending if is currently completed
                var taskDomainModel = await taskRepository.GetByIdAsync(updatePhaseRequestDto.TaskId);
                if(taskDomainModel != null)
                {
                    if(taskDomainModel.Status == Enums.TaskStatus.Completed)
                    {
                        taskDomainModel.Status = Enums.TaskStatus.Pending;
                        await taskRepository.UpdateAsync(updatePhaseRequestDto.TaskId, taskDomainModel);
                    }
                }
            }

            var phaseDto = mapper.Map<PhaseDto>(phaseDomainModel);

            return Ok(phaseDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedPhaseDomainModel = await phaseRepository.DeleteAsync(id);

            if (deletedPhaseDomainModel == null)
                return NotFound();

            // get all by task id
            var phasesDomainModel = await phaseRepository.GetAllByTaskIdAsync(deletedPhaseDomainModel.TaskId);

            // order by number
            phasesDomainModel = phasesDomainModel.OrderBy(p => p.Number).ToList();

            // update number
            for (int i = 0; i < phasesDomainModel.Count; i++)
            {
                phasesDomainModel[i].Number = i + 1;
            }

            // update all phases
            foreach (var phase in phasesDomainModel)
            {
                await phaseRepository.UpdateAsync(phase.id, phase);
            }

            var deletedPhaseDto = mapper.Map<PhaseDto>(deletedPhaseDomainModel);

            return Ok(deletedPhaseDto);
        }

        [HttpDelete]
        [Route("ByTaskId/{taskId:Guid}")]
        public async Task<IActionResult> DeleteAllByTaskId([FromRoute] Guid taskId)
        {
            var phasesDomainModel = await phaseRepository.GetAllByTaskIdAsync(taskId);

            var deletedPhasesDomainModel = new List<Phase>();

            foreach (var phase in phasesDomainModel)
            {
                var deletedPhaseDomainModel = await phaseRepository.DeleteAsync(phase.id);
                if (deletedPhaseDomainModel != null)
                    deletedPhasesDomainModel.Add(deletedPhaseDomainModel);
            }

            var deletedPhasesDto = mapper.Map<List<PhaseDto>>(deletedPhasesDomainModel);

            return Ok(deletedPhasesDto);
        }

    }
}
