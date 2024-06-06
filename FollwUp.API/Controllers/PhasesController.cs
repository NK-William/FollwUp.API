using AutoMapper;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhaseController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPhaseRepository phaseRepository;

        public PhaseController(IMapper mapper, IPhaseRepository phaseRepository)
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
    }
}
