using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var users = await _userService.GetUserByIdAsync(id);

            if(users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        /// <summary>
        /// Updates users by given FromBody input
        /// </summary>
        /// <param name="id">UserID (string)</param>
        /// <param name="dto">[FromBody] json</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UpdateUserDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var isSuccess = await _userService.DeleteUserAsync(id);
            
            if(!isSuccess)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {

            Console.WriteLine("\n\n\n\n\nClaim Types:");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type}");

            }
            Console.WriteLine("\n\n\n\n\n");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new { userId, email });
        }

        [HttpGet("get-all-information")]
        public async Task<IActionResult> GetUserInformation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userInformation = await _userService.GetUserInformation(userId);

            if(userInformation == null)
            {
                return BadRequest("User was not found.");
            }

            return Ok(userInformation);

        }


    }
}
