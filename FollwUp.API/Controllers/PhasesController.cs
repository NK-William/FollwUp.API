using AutoMapper;
using FollwUp.API.Enums;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhasesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPhaseRepository phaseRepository;

        public PhasesController(IMapper mapper, IPhaseRepository phaseRepository)
        {
            this.mapper = mapper;
            this.phaseRepository = phaseRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPhaseRequestDto addPhaseRequestDto)
        {

            var phaseDomainModel = mapper.Map<Phase>(addPhaseRequestDto);

            await phaseRepository.CreateAsync(phaseDomainModel);

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

                        // Editing old phase
                        phasesDomainModel[i + 1] = container;
                        phasesDomainModel[i + 1].Number = currentNumber + 1;
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
            }


            var phaseDto = mapper.Map<PhaseDto>(phaseDomainModel);

            return Ok(phaseDto);
        }

        [HttpPost]
        [Route("List")]
        public async Task<IActionResult> CreatePhases([FromBody] List<AddPhaseRequestDto> addPhasesRequestDto)
        {
            var phasesDomainModel = mapper.Map<List<Phase>>(addPhasesRequestDto);

            await phaseRepository.CreatePhasesAsync(phasesDomainModel);

            var phasesDto = mapper.Map<List<PhaseDto>>(phasesDomainModel);

            return Ok(phasesDto);
        }

        [HttpGet]
        [Route("ByTaskId/{id:Guid}")]
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
            }

            var phaseDto = mapper.Map<PhaseDto>(phaseDomainModel);

            return Ok(phaseDto);
        }

        // ToDo: Next implement the Delete method
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

    }
}
