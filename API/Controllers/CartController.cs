using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    //[Route("api/[controller]")]
    [Route("cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;
        private readonly IStripeService _stripeService;
        private readonly IItineraryService _itineraryService;

        public CartController(
            ILogger<CartController> logger,
            ICartService cartService,
            IStripeService stripeService,
            IItineraryService itineraryService)
        {
            _logger = logger;
            _cartService = cartService;
            _stripeService = stripeService;
            _itineraryService = itineraryService;
        }

        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddToCart(string userId, [FromBody] AddToCartRequest req)
        {
            await _cartService.AddToCartAsync(userId, req.TourId, req.SeatsBooked);
            return Ok();
        }

        [HttpGet("get-all/{userId}")]
        public async Task<ActionResult<List<CartItemDto>>> GetCart(string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);

            return Ok(cart);
        }

        [HttpDelete("remove/{userId}/{bookingId}")]
        public async Task<IActionResult> RemoveFromCart(string userId, Guid bookingId)
        {
            await _cartService.RemoveFromCartAsync(userId, bookingId);
            return Ok();
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            await _cartService.ClearCartAsync(userId);

            return Ok();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            var userId = request.UserId ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cartItems = await _cartService.GetCartAsync(userId);

            if(cartItems == null || !cartItems.Any())
            {
                return BadRequest("Cart is empty");
            }

            foreach (var item in cartItems)
            {
                var itinerary = new CreateItineraryDto
                {
                    UserId = userId,
                    TourId = item.TourId,
                    SeatsBooked = item.SeatsBooked
                };

                await _itineraryService.CreateItineraryAsync(itinerary);
                await _cartService.RemoveFromCartAsync(userId, item.BookingId);
            }

            

            return Ok("Itinerary created.");
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cartItems = await _cartService.GetCartAsync(userId);

            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty.");
            }

            var stripeSession = await _stripeService.CreateCheckoutSession(cartItems, userId);

            return Ok(new
            {
                url = stripeSession.Url
            });
        }
    }
}

      