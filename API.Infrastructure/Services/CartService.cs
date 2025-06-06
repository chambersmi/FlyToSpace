using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Application.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace API.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IDatabase _database;
        private readonly ILogger<CartService> _logger;
        private readonly ITourService _tourService;
        private readonly IItineraryService _itineraryService;

        public CartService(
            ILogger<CartService> logger,
            IConnectionMultiplexer redis,
            ITourService tourService,
            IItineraryService itineraryService)
        {
            _database = redis.GetDatabase();
            _logger = logger;
            _tourService = tourService;
            _itineraryService = itineraryService;
        }

        public async Task AddToCartAsync(string userId, int tourId, int seatsBooked)
        {
            var tour = await _tourService.GetTourByIdAsync(tourId);
            if(tour == null)
            {
                _logger.LogWarning($"Tour with ID {tourId} could not be found.");
                return;
            }

            // Use automapper for this after test
            var cartItem = new CartItemDto
            {
                TourId = tourId,
                TourName = tour.TourName,
                TourPrice = tour.TourPrice,
                SeatsBooked = seatsBooked,
                // Eventually use calculate total price method
                TotalPrice = await _itineraryService.GetTotalPriceAsync(tourId)
            };

            var json = JsonSerializer.Serialize(cartItem);
            await _database.ListRightPushAsync($"cart:{userId}", json);
        }

        public async Task ClearCartAsync(string userId)
        {
            await _database.KeyDeleteAsync($"cart:{userId}");
        }

        public async Task<IEnumerable<CartItemDto?>> GetCartAsync(string userId)
        {
            var key = $"cart:{userId}";
            var items = await _database.ListRangeAsync(key);

            return items
                .Select(x => JsonSerializer.Deserialize<CartItemDto>(x!))
                .Where(x => x != null)
                .ToList()!;
        }

        public async Task RemoveFromCartAsync(string userId, Guid bookingId)
        {
            var redisKey = $"cart:{userId}";
            var items = await _database.ListRangeAsync(redisKey);
            if (items.Length == 0) return;

            foreach(var item in items)
            {
                var cartItem = JsonSerializer.Deserialize<CartItemDto>(item!);
                if (cartItem != null && cartItem.BookingId == bookingId)
                {
                    await _database.ListRemoveAsync(redisKey, item);
                    break;
                }
            }
        }
    }
}
