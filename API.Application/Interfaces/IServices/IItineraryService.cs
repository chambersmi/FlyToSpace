using API.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Interfaces.IServices
{
    public interface IItineraryService
    {
        Task<IEnumerable<BookingDto>> GetAllItinerariesByUserIdAsync(string userId);
        Task<BookingDto?> GetSingleUserItineraryByIdAsync(int id, string userId);
        Task<bool> DeleteItineraryByIdAsync(int id);
        Task<BookingDto?> UpdateItineraryAsync(int id, UpdateItineraryDto dto);
        Task<BookingDto?> CreateItineraryAsync(CreateItineraryDto dto);

        Task<IEnumerable<BookingDto>> GetAllItinerariesAsync();
        Task<BookingDto> GetItineraryByIdAsync(int id);
        Task<decimal> GetTotalPriceAsync(int id);
    }
}
