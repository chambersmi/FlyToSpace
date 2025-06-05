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
    public class ItineraryController : ControllerBase
    {
        private readonly ILogger<ItineraryController> _logger;
        private readonly IItineraryService _itineraryService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public ItineraryController(
            ILogger<ItineraryController> logger,
            IItineraryService itineraryService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUserService userService
            )
        {
            _logger = logger;
            _itineraryService = itineraryService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }

        /// <summary>
        /// Returns all itineraries by user
        /// </summary>
        /// <returns></returns>
        [HttpGet("user/all")]
        public async Task<IActionResult> GetAllItinerariesByUserIdAsync()
        {
            //var user = await _userManager.GetUserAsync(User);
            var userId = User.FindFirst("id")?.Value;
            var email = User.FindFirst("email")?.Value;
            
           if(userId == null)
            {
                return NotFound();
            }

            var itineraries = await _itineraryService.GetAllItinerariesByUserIdAsync(userId);
            return Ok(itineraries);
        }

        /// <summary>
        /// Return a single itinerary by a user
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetSingleUserItineraryByIdAsync(int itineraryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            
            if(userId == null)
            {
                return Unauthorized();
            }

            var itinerary = await _itineraryService.GetSingleUserItineraryByIdAsync(itineraryId, userId);

            if(itinerary == null)
            {
                return NotFound($"{itineraryId} not found.");
            }

            return Ok(itinerary);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateItineraryAsync([FromBody] CreateItineraryDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _itineraryService.CreateItineraryAsync(dto);

            return Ok(dto);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBookingAsync(int id, [FromBody] UpdateItineraryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itinerary = await _itineraryService.UpdateItineraryAsync(id, dto);

            if (itinerary == null)
                return BadRequest($"{itinerary.ItineraryId} not found.");

            return Ok(itinerary);

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItineraryAsync(int id)
        {
            var isSuccess = await _itineraryService.DeleteItineraryByIdAsync(id);

            if(!isSuccess)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllItinerariesAsync()
        {
            var itineraries = await _itineraryService.GetAllItinerariesAsync();
            return Ok(itineraries);
        }

        [HttpGet("total-price/{id}")]
        public async Task<IActionResult> GetTotalPriceAsync(int id)
        {
            decimal? totalPrice = await _itineraryService.GetTotalPriceAsync(id);

            if(totalPrice == null)
            {
                return NotFound();
            }

            return Ok(totalPrice);
        }
    }
}
