using API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Interfaces.IRepositories
{
    public interface ITourRepository
    {
        Task<Tour> GetTourByIdAsync(int id);
        Task<List<Tour>> GetAllToursAsync();
        Task UpdateTourAsync(Tour tour);
        Task<bool> DeleteTourAsync(int id);
        Task<Tour> CreateTourAsync(Tour tour);
    }
}
