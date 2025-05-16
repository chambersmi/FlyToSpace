using API.Application.Interfaces;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserRepository(ILogger<UserRepository> logger,
            UserManager<ApplicationUser> userManager,
            AppDbContext context
            )
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
