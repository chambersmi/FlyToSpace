using API.Domains.DTOs;

namespace API.Application.Services
{
    public interface IUserService
    {
        Task<List<UserRequestDto>> GetAllUsersAsync();
    }
}