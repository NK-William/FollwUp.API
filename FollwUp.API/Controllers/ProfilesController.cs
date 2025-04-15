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

        [HttpGet]
        [Route("ByEmail/{email}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var profileDomainModel = await profileRepository.GetByEmailAsync(email);

            if (profileDomainModel == null)
                return NotFound("Profile not found");

            var profileDto = mapper.Map<ProfileDto>(profileDomainModel);

            return Ok(profileDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProfileRequestDto updateProfileRequestDto)
        {
            var profileDomainModel = mapper.Map<Model.Domain.Profile>(updateProfileRequestDto);

            var updatedProfileDomainModel= await profileRepository.UpdateAsync(profileDomainModel);

            if (updatedProfileDomainModel == null)
                return NotFound("Profile not found");

            var profileDto = mapper.Map<ProfileDto>(updatedProfileDomainModel);

            return Ok(profileDto);
        }
    }
}
