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
    public class TourRepository : ITourRepository
    {
        private readonly ILogger<TourRepository> _logger;
        private readonly AppDbContext _context;

        public TourRepository(ILogger<TourRepository> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> DeleteTourAsync(int id)
        {
            var tourId = await _context.Tours.FirstOrDefaultAsync(x => x.TourId == id);

            if(tourId == null)
            {
                return false;
            }

            _context.Tours.Remove(tourId);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Tour>> GetAllToursAsync()
        {
            return await _context.Tours.ToListAsync();
        }

        public async Task<Tour> GetTourByIdAsync(int id)
        {
            var tourEntity = await _context.Tours.FirstOrDefaultAsync(x => x.TourId == id);            

            return tourEntity;
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
        }

        public async Task<Tour> CreateTourAsync(Tour tour)
        {
            await _context.Tours.AddAsync(tour);
            await _context.SaveChangesAsync();

            return tour;
        }
    }
}
