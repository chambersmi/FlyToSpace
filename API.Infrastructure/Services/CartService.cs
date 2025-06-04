using API.Application.DTOs;
using API.Application.Interfaces.IServices;
using API.Application.ViewModels;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IDatabase _database;
        private readonly ILogger<CartService> _logger;
        private readonly ITourService _tourService;

        public CartService(
            ILogger<CartService> logger, 
            IConnectionMultiplexer redis,
            ITourService tourService)
        {
            _database = redis.GetDatabase();
            _logger = logger;
            _tourService = tourService;
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
                TotalPrice = seatsBooked * tour.TourPrice
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

        public async Task RemoveFromCartAsync(string userId, int tourId)
        {
            var redisKey = $"cart:{userId}";
            var cartData = await _database.StringGetAsync(redisKey);

            if (!cartData.HasValue)
                return;

            var cartItems = JsonSerializer.Deserialize<List<CartItemDto>>(cartData!);

            var updatedCart = cartItems
                .Where(item => item.TourId != tourId)
                .ToList();

            await _database.StringSetAsync(redisKey, JsonSerializer.Serialize(updatedCart));
        }
    }
}
