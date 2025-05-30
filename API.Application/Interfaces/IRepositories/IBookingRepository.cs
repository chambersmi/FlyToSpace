using API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Interfaces.IRepositories
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllBookingsByUserIdAsync(string userId);
        Task<Booking> GetUserBookingByIdAsync(int id, string userId);
        Task UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<int> GetSeatsBookedCountAsync(int tourId);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
    }
}
