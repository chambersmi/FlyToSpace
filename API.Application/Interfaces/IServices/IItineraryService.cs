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
        Task<IEnumerable<ItineraryDto>> GetAllItinerariesByUserIdAsync(string userId);
        Task<ItineraryDto?> GetSingleUserItineraryByIdAsync(int id, string userId);
        Task<bool> DeleteItineraryByIdAsync(int id);
        Task<ItineraryDto?> UpdateItineraryAsync(int id, UpdateItineraryDto dto);
        Task<ItineraryDto?> CreateItineraryAsync(CreateItineraryDto dto);

        Task<IEnumerable<ItineraryDto>> GetAllItinerariesAsync();
        Task<ItineraryDto> GetItineraryByIdAsync(int id);
        Task<decimal> GetTotalPriceAsync(int id);
    }
}
