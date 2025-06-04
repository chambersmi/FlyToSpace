using API.Application.DTOs;
using API.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Interfaces.IServices
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, int tourId, int seatsBooked);
        Task<IEnumerable<CartItemDto?>> GetCartAsync(string userId);
        Task ClearCartAsync(string userId);
        Task RemoveFromCartAsync(string userId, Guid bookingId);
    }
}
