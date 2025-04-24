using API.Domains.DTOs;
using API.Domains.Models;
using API.Infrastructure.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthService> logger,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {                
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Birthdate = dto.Birthdate,
                PhoneNumber = dto.PhoneNumber,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if(!result.Succeeded)
            {
                _logger.LogError("Error: AuthService");
                return AuthResponseDto.Failure(result.Errors.Select(e => e.Description));
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Role_User);

            var token = _tokenService.CreateToken(user);
            return AuthResponseDto.Success(user, token);            
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if(user == null)
            {
                _logger.LogInformation($"Invalid email or password: {dto.Email}");
                return AuthResponseDto.Failure("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if(!result.Succeeded)
            {
                _logger.LogInformation($"Invalid email or password: {dto.Email}");
                return AuthResponseDto.Failure("Invalid email or password.");
            }

            var token = _tokenService.CreateToken(user);
            return AuthResponseDto.Success(user, token);
        }

        public async Task<AuthResponseDto> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            var email = userPrincipal.FindFirstValue(ClaimTypes.Email);
            if(email == null)
            {
                return AuthResponseDto.Failure("User not found.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return AuthResponseDto.Failure("User not found.");
            }

            return AuthResponseDto.Success(user);
        }
    }
}
