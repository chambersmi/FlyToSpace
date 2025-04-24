using API.Domains.DTOs;
using System.Security.Claims;

namespace API.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> GetCurrentUserAsync(ClaimsPrincipal user);
    }
}