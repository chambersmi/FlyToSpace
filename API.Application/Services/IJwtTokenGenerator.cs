using API.Domain.Entities;

namespace API.Infrastructure.Auth
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}