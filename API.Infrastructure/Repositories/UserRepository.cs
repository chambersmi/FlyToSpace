using API.Application.Interfaces.IRepositories;
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
        private readonly AppDbContext _context;

        public UserRepository(ILogger<UserRepository> logger,
            AppDbContext context
            )
        {
            _logger = logger;
            _context = context;
            
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userToDelete == null) return false;

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return true;
        }
            

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(b => b.Bookings)
                .ThenInclude(t => t.Tour)
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Include(b => b.Bookings)
                .ThenInclude(t => t.Tour)
                .FirstOrDefaultAsync(x => x.Id == id);            
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
