using AutoMapper;
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
        public async Task<IActionResult> Create([FromBody] List<AddPhaseRequestDto> addPhasesRequestDto)
        {
            var phasesDomainModel = mapper.Map<List<Phase>>(addPhasesRequestDto);

            await phaseRepository.CreateAsync(phasesDomainModel);

            var phasesDto = mapper.Map<List<PhaseDto>>(phasesDomainModel);

            return Ok(phasesDto);
        }

        [HttpGet]
        [Route("byTaskId/{id:Guid}")]
        public async Task<IActionResult> GetAllByTaskId([FromRoute] Guid id)
        { 
            var phasesDomainModel = await phaseRepository.GetAllByTaskIdAsync(id);

            var phasesDto = mapper.Map<List<PhaseDto>>(phasesDomainModel);

            return Ok(phasesDto);
        }
    }
}
