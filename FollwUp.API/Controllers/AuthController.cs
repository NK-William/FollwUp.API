﻿using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly ProfilesController profilesController;

        public AuthController(UserManager<IdentityUser> usermanager, ITokenRepository tokenRepository, ProfilesController profilesController)
        {
            this.userManager = usermanager;
            this.tokenRepository = tokenRepository;
            this.profilesController = profilesController;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                var profileRequestDto = new AddProfileRequestDto
                {
                   EmailAddress = registerRequestDto.Email,
                   FirstName = registerRequestDto.FirstName,
                   LastName = registerRequestDto.LastName,
                   PhoneNumber = registerRequestDto.PhoneNumber
                };

                await profilesController.Create(profileRequestDto);

                return Ok("User created successfully");
            }

            return BadRequest(identityResult.Errors);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var identityUser = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (identityUser != null && await userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password))
            {
                // Create Token
                var jwtToken = tokenRepository.CreateJWTToken(identityUser);

                var response = new LoginResponseDto
                {
                    JwtToken = jwtToken,
                };

                return Ok(response);

            }
            return BadRequest("Username or password incorrect");
        }
    }
}
