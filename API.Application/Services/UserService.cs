using API.Application.DTOs;
using API.Application.Interfaces.IRepositories;
using API.Application.Interfaces.IServices;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace API.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            ILogger<UserService> logger, 
            IUserRepository userRepository,
            IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteUserAsync(user.Id);

            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUserAsync(string id, UpdateUserDto dto)
        {
            var userToUpdate = await _userRepository.GetUserByIdAsync(id);
            if(userToUpdate == null)
            {
                return false;
            }

            _mapper.Map(dto, userToUpdate);

            await _userRepository.UpdateUserAsync(userToUpdate);

            return true;
        }

        public async Task<CheckoutRequestDto> GetUserInformation(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            return new CheckoutRequestDto
            {
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                StreetAddress1 = user.StreetAddress1,
                StreetAddress2 = user.StreetAddress2,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            };
        }
    }
}
