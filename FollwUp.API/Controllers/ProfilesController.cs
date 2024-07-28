using AutoMapper;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProfileRepository profileRepository;

        public ProfilesController(IMapper mapper, IProfileRepository profileRepository)
        {
            this.mapper = mapper;
            this.profileRepository = profileRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProfileRequestDto addProfileRequestDto)
        {
            var profileDomainModel = mapper.Map<Model.Domain.Profile>(addProfileRequestDto);

            await profileRepository.CreateAsync(profileDomainModel);

            var profileDto = mapper.Map<ProfileDto>(profileDomainModel);

            return Ok(profileDto);
        }
    }
}
