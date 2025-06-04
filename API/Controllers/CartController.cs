using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public CartController(
            ILogger<CartController> logger,
            ICartService cartService,
            IStripeService stripeService)
        {
            _logger = logger;
            _cartService = cartService;
            _stripeService = stripeService;
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
        public async Task<IActionResult> Checkout(string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            if (!cart.Any())
            {
                return BadRequest("Cart is empty.");
            }

            var stripeSession = await _stripeService.CreateCheckoutSession(cart, userId);

            return Ok(new
            {
                sessionId = stripeSession.Id
            });
        }
    }
}

      