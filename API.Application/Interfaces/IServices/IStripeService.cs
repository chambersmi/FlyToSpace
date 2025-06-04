using API.Application.DTOs;
using Stripe.Checkout;

namespace API.Application.Interfaces.IServices
{
    public interface IStripeService
    {
        Task<Session> CreateCheckoutSession(IEnumerable<CartItemDto> cartItems, string userId);
    }
}
