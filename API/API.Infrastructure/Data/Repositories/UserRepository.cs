using API.Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Data.Repositories
{
    
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FTSDbContext _context;

        public UserRepository(ILogger<UserRepository> logger, 
            UserManager<ApplicationUser> userManager,
            FTSDbContext context
            )
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> DeleteUserByIdAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if(user != null)
            {
                await _userManager.DeleteAsync(user);
                return true;
            }

            return false;
        }
    }
}
