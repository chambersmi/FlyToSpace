using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Domain.Entities;
using API.Infrastructure.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace API.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthService(
            ILogger<AuthService> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<(bool Success, string? Token)> AuthenticateUserAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if(user == null)
            {
                return (false, null);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            
            if(result.Succeeded)
            {
                var token = await _jwtTokenGenerator.GenerateToken(user);
                return (true, token);
            }

            
            return (false, null);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            
            if(dto.Password != dto.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordMismatch",
                    Description = "Passwords must match."
                });
            }

            if(!string.IsNullOrEmpty(dto.Email))
            {
                var emailExists = await _userManager.FindByEmailAsync(dto.Email);
                if(emailExists != null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateEmail",
                        Description = "Invalid email in use."
                    });
                }
            }          

            var user = _mapper.Map<ApplicationUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            await _userManager.AddToRoleAsync(user, dto.Role);

            return result;
        }
    }
}
