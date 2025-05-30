using API.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Interfaces.IServices
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllUserBookingsAsync(string userId);
        Task<BookingDto?> GetUserBookingByIdAsync(int id, string userId);
        Task<bool> DeleteBookingByIdAsync(int id);
        Task<BookingDto?> UpdateBookingAsync(int id, UpdateBookingDto dto);
        Task<BookingDto?> CreateBookingAsync(CreateBookingDto dto);

        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(int id);
    }
}
