using API.Domains.Models;

namespace API.Application.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}