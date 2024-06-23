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
    public class RolesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;

        public RolesController(IMapper mapper, IRoleRepository roleRepository)
        {
            this.mapper = mapper;
            this.roleRepository = roleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRoleRequestDto addRoleRequestDto)
        {
            var roleDomainModel = mapper.Map<Role>(addRoleRequestDto);

            await roleRepository.CreateAsync(roleDomainModel);

            var roleDto = mapper.Map<RoleDto>(roleDomainModel);

            return Ok(roleDto);
        }

        [HttpGet]
        [Route("byProfileId/{id:Guid}")]
        public async Task<IActionResult> GetAllByProfileId([FromRoute] Guid id)
        {
            var rolesDomainModel = await roleRepository.GetAllByProfileIdAsync(id);

            var rolesDto = mapper.Map<List<RoleDto>>(rolesDomainModel);

            return Ok(rolesDto);
        }

        [HttpGet]
        [Route("byTaskId/{id:Guid}")]
        public async Task<IActionResult> GetAllByTaskId([FromRoute] Guid id)
        {
            var rolesDomainModel = await roleRepository.GetAllByTaskIdAsync(id);

            var rolesDto = mapper.Map<List<RoleDto>>(rolesDomainModel);

            return Ok(rolesDto);
        }
    }
}
