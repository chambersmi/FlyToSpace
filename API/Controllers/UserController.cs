using API.Application.DTOs;
using API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all users from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves one user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var users = await _userService.GetUserByIdAsync(id);
            return Ok(users);
        }

        /// <summary>
        /// Updates users by given FromBody input
        /// </summary>
        /// <param name="id">UserID (string)</param>
        /// <param name="dto">[FromBody] json</param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UpdateUserDto dto)
        {
            var isSuccess = await _userService.UpdateUserAsync(id, dto);
            
            if(!isSuccess)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Deletes one user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var isSuccess = await _userService.DeleteUserAsync(id);
            
            if(!isSuccess)
            {
                return NotFound();
            }

            return Ok();
        }


    }
}
