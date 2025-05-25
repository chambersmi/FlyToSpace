using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(
            ILogger<BookingController> logger,
            IBookingService bookingService,
            IMapper mapper)
        {
            _logger = logger;
            _bookingService = bookingService;
            _mapper = mapper;                
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if(booking == null)
            {
                return NotFound($"{id} not found.");
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
    }
}
