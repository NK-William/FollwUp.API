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
        [Route("ByTaskId/{id:Guid}")]
        public async Task<IActionResult> GetByTaskId([FromRoute] Guid id)
        {
            var invitationDomainModel = await invitationRepository.GetByTaskIdAsync(id);

            var invitationDto = mapper.Map<InvitationDto>(invitationDomainModel);

            return Ok(invitationDto);
        }
    }
}
