using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Application.Services;
using API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace API.Controllers
{
    [ApiController]
    [Route("api/stripe/webhook")]
    public class StripeWebhookController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ICartService _cartService;
        private readonly ITourService _tourService;
        private readonly IItineraryService _itineraryService;
        private readonly ILogger<StripeWebhookController> _logger;

        public StripeWebhookController(
            IConfiguration config,
            ICartService cartService,
            ITourService tourService,
            ILogger<StripeWebhookController> logger)
        {
            _config = config;
            _cartService = cartService;
            _tourService = tourService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _config["Stripe:NA"]);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    var userId = session.Metadata["userId"];

                    var cart = await _cartService.GetCartAsync(userId);

                    foreach (var item in cart)
                    {
                        var dto = new CreateItineraryDto
                        {
                            UserId = userId,
                            TourId = item.TourId,
                            SeatsBooked = item.SeatsBooked,
                        };

                        await _itineraryService.CreateItineraryAsync(dto);
                        await _cartService.RemoveFromCartAsync(userId, item.BookingId);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Stripe Webhook Error: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
