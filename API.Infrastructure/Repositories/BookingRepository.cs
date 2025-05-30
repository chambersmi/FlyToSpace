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
    public class BookingRepository : IBookingRepository
    {
        private readonly ILogger<TourRepository> _logger;
        private readonly AppDbContext _context;

        public BookingRepository(ILogger<TourRepository> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            await _context.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var bookingId = await _context.Bookings.FirstOrDefaultAsync(x => x.BookingId == id);

            if (bookingId == null)
                return false;

            _context.Bookings.Remove(bookingId);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// This will retrieve ALL bookings by the user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Booking>> GetAllBookingsByUserIdAsync(string userId)
        {
            return await _context.Bookings
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
        public async Task<Booking> GetUserBookingByIdAsync(int id, string userId)
        {
            var bookingEntity = await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.User)                
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync(x => x.BookingId == id);

            return bookingEntity;
        }

        public async Task<int> GetSeatsBookedCountAsync(int tourId)
        {
            return await _context.Bookings
                .Where(t => t.TourId == tourId)
                .SumAsync(b => b.SeatsBooked);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            var bookingEntity = await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.User)
                .ToListAsync();

            return bookingEntity;
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            var bookingEntity = await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.User)
                .FirstOrDefaultAsync(x => x.BookingId == id);

            return bookingEntity;
        }
        
    }
}
