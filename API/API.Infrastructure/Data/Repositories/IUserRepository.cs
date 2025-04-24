using API.Domains.Models;

namespace API.Infrastructure.Data.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<bool> DeleteUserByIdAsync(string userId);
    }
}