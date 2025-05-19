using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Domain.Entities;
using API.Infrastructure.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthController(ILogger<AuthController> logger,
            IAuthService authService,
            IJwtTokenGenerator jwtTokenGenerator
            )
        {
            _logger = logger;
            _authService = authService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          
            var result = await _authService.RegisterAsync(model);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(result.Errors);
            }

            return Ok("User successfully created.");

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var (success, user) = await _authService.AuthenticateUserAsync(model);

                if (!success || user == null)
                {
                    return Unauthorized("Invalid Credentials.");
                }

                var token = _jwtTokenGenerator.GenerateToken(user);
                return Ok(new
                {
                    token
                });
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error:\n{ex.Message}");
            }

        }        
    }
}
