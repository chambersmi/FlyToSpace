using API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Interfaces.IRepositories
{
    public interface IItineraryRepository
    {
        Task<List<Itinerary>> GetAllItinerariesByUserIdAsync(string userId);
        Task<Itinerary> GetSingleUserItineraryByIdAsync(int id, string userId);
        Task UpdateItineraryAsync(Itinerary booking);
        Task<bool> DeleteItineraryByIdAsync(int id);
        Task<Itinerary> CreateItineraryAsync(Itinerary booking);
        Task<int> GetSeatsBookedCountAsync(int tourId);
        Task<List<Itinerary>> GetAllItinerariesAsync();
        Task<Itinerary> GetItineraryByIdAsync(int id);
    }
}
