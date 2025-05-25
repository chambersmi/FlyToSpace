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
        Task<Booking> GetBookingByIdAsync(int id);
        Task<List<Booking>> GetAllBookingsAsync();
        Task UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
    }
}
