using API.Application.DTOs;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<(bool Success, string? Token)> AuthenticateUserAsync(LoginDto dto);
    }
}
