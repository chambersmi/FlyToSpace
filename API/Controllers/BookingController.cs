using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Domain.Entities;
using API.Infrastructure.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public BookingController(
            ILogger<BookingController> logger,
            IBookingService bookingService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUserService userService
            )
        {
            _logger = logger;
            _bookingService = bookingService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }

        
        [HttpGet("itinerary")]
        public async Task<IActionResult> GetAllUserBookingsAsync()
        {
            //var user = await _userManager.GetUserAsync(User);
            var userId = User.FindFirst("id")?.Value;
            var email = User.FindFirst("email")?.Value;
            
           if(userId == null)
            {
                return NotFound();
            }

            var bookings = await _bookingService.GetAllUserBookingsAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserBookingsByIdAsync(int bookingId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            
            if(userId == null)
            {
                return Unauthorized();
            }

            var booking = await _bookingService.GetUserBookingByIdAsync(bookingId, userId);

            if(booking == null)
            {
                return NotFound($"{bookingId} not found.");
            }

            return Ok(booking);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookingService.CreateBookingAsync(dto);

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookingAsync(int id, [FromBody] UpdateBookingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _bookingService.UpdateBookingAsync(id, dto);

            if (booking == null)
                return BadRequest($"{booking.BookingId} not found.");

            return Ok(booking);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingAsync(int id)
        {
            var isSuccess = await _bookingService.DeleteBookingByIdAsync(id);

            if(!isSuccess)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
    }
}
