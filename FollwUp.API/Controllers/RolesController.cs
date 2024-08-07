﻿using AutoMapper;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            if(addRoleRequestDto.TaskId.Equals(Guid.Empty))
                return BadRequest("TaskId must be valid.");

            if (addRoleRequestDto.ProfileId.Equals(Guid.Empty))
                return BadRequest("ProfileId must be valid.");

            var roleDomainModel = mapper.Map<Role>(addRoleRequestDto);

            await roleRepository.CreateAsync(roleDomainModel);

            var roleDto = mapper.Map<RoleDto>(roleDomainModel);

            return Ok(roleDto);
        }

        [HttpGet]
        [Route("ByProfileId/{id:Guid}")]
        public async Task<IActionResult> GetAllByProfileId([FromRoute] Guid id)
        {
            var rolesDomainModel = await roleRepository.GetAllByProfileIdAsync(id);

            var rolesDto = mapper.Map<List<RoleDto>>(rolesDomainModel);

            return Ok(rolesDto);
        }

        [HttpGet]
        [Route("ByTaskId/{id:Guid}")]
        public async Task<IActionResult> GetAllByTaskId([FromRoute] Guid id)
        {
            var rolesDomainModel = await roleRepository.GetAllByTaskIdAsync(id);

            var rolesDto = mapper.Map<List<RoleDto>>(rolesDomainModel);

            return Ok(rolesDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var roleDomainModel = await roleRepository.DeleteAsync(id);

            var roleDto = mapper.Map<RoleDto>(roleDomainModel);

            return Ok(roleDto);
        }

        [HttpDelete]
        [Route("ByTaskId/{taskId:Guid}")]
        public async Task<IActionResult> DeleteAllByTaskId([FromRoute] Guid taskId)
        {
            var rolesDomainModel = await roleRepository.GetAllByTaskIdAsync(taskId);

            var deletedRolesDomainModel = new List<Role>();

            foreach (var role in rolesDomainModel)
            {
                var deletedRoleDomainModel = await roleRepository.DeleteAsync(role.Id);
                if (deletedRoleDomainModel != null)
                    deletedRolesDomainModel.Add(deletedRoleDomainModel);
            }

            var deletedRolesDto = mapper.Map<List<Role>>(deletedRolesDomainModel);

            return Ok(deletedRolesDto);
        }
    }
}
