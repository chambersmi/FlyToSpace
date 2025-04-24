using API.Domains.DTOs;
using API.Domains.Models;
using API.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<List<UserRequestDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            var userDTO = users.Select(user => new UserRequestDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Birthdate = user.Birthdate,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            }).ToList();

            return userDTO;
        }

        public async Task<UserRequestDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            var userDTO = new UserRequestDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Birthdate = user.Birthdate,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                PhoneNumber = user.PhoneNumber
            };

            return userDTO;
        }
    }
}
