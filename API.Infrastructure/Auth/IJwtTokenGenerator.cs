using API.Domain.Entities;

namespace API.Infrastructure.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}