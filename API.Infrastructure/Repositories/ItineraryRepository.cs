using API.Application.Interfaces.IRepositories;
using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Repositories
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly ILogger<ItineraryRepository> _logger;
        private readonly AppDbContext _context;

        public ItineraryRepository(ILogger<ItineraryRepository> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Itinerary> CreateItineraryAsync(Itinerary booking)
        {
            await _context.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> DeleteItineraryByIdAsync(int id)
        {
            var bookingId = await _context.Itineraries.FirstOrDefaultAsync(x => x.ItineraryId == id);

            if (bookingId == null)
                return false;

            _context.Itineraries.Remove(bookingId);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// This will retrieve ALL bookings by the user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Itinerary>> GetAllItinerariesByUserIdAsync(string userId)
        {
            return await _context.Itineraries
                .Include(b => b.Tour)
                .Include(b => b.User)
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// This will retrieve a single booking by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Itinerary> GetSingleUserItineraryByIdAsync(int id, string userId)
        {
            var bookingEntity = await _context.Itineraries
                .Include(b => b.Tour)
                .Include(b => b.User)                
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync(x => x.ItineraryId == id);

            return bookingEntity;
        }

        public async Task<int> GetSeatsBookedCountAsync(int tourId)
        {
            return await _context.Itineraries
                .Where(t => t.TourId == tourId)
                .SumAsync(b => b.SeatsBooked);
        }

        public async Task UpdateItineraryAsync(Itinerary booking)
        {
            _context.Itineraries.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Itinerary>> GetAllItinerariesAsync()
        {
            var bookingEntity = await _context.Itineraries
                .Include(b => b.Tour)
                .Include(b => b.User)
                .ToListAsync();

            return bookingEntity;
        }

        public async Task<Itinerary> GetItineraryByIdAsync(int id)
        {
            var bookingEntity = await _context.Itineraries
                .Include(b => b.Tour)
                .Include(b => b.User)
                .FirstOrDefaultAsync(x => x.ItineraryId == id);

            return bookingEntity;
        }
        
    }
}
