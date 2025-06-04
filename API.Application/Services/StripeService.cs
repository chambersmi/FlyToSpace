using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;


namespace API.Application.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StripeService> _logger;

        public StripeService(ILogger<StripeService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<Session> CreateCheckoutSession(IEnumerable<CartItemDto> cartItems, string userId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmountDecimal = item.TourPrice * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.TourName
                        }
                    },
                    Quantity = item.SeatsBooked
                }).ToList(),
                Mode = "payment",
                SuccessUrl = _configuration["Stripe:SuccessUrl"],
                CancelUrl = _configuration["Stripe:CancelUrl"],
                Metadata = new Dictionary<string, string>
                {
                    { "userId", userId }
                }

            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }
    }
}
