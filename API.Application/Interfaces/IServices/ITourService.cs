using API.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Interfaces.IServices
{
    public interface ITourService
    {
        Task<IEnumerable<TourDto>> GetAllToursAsync();
        Task<TourDto?> GetTourByIdAsync(int id);
        Task<bool> DeleteTourByIdAsync(int id);
        Task<TourDto> UpdateTourAsync(int id, UpdateTourDto dto);
        Task<TourDto?> CreateTourAsync(CreateTourDto dto);

    }
}
