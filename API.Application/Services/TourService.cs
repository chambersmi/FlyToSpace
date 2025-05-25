using API.Application.DTOs;
using API.Application.Interfaces.IRepositories;
using API.Application.Interfaces.IServices;
using API.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Services
{
    public class TourService : ITourService
    {
        private readonly ILogger<TourService> _logger;
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;

        public TourService(
            ILogger<TourService> logger, 
            ITourRepository tourRepository,
            IMapper mapper)
        {
            _logger = logger;
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteTourByIdAsync(int id)
        {
            var tourToDelete = await _tourRepository.GetTourByIdAsync(id);
            
            if (tourToDelete == null)
                return false;

            await _tourRepository.DeleteTourAsync(tourToDelete.TourId);

            return true;
        }

        public async Task<IEnumerable<TourDto>> GetAllToursAsync()
        {
            var tours = await _tourRepository.GetAllToursAsync();
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }

        public async Task<TourDto?> GetTourByIdAsync(int id)
        {
            var tour = await _tourRepository.GetTourByIdAsync(id);
            return _mapper.Map<TourDto>(tour);
        }

        public async Task<TourDto?> CreateTourAsync(CreateTourDto dto)
        {
            var tourEntity = _mapper.Map<Tour>(dto);
            var tour = await _tourRepository.CreateTourAsync(tourEntity);
            var resultDto = _mapper.Map<TourDto>(tour);

            return resultDto;

        }

        // Change return type to TourDto
        public async Task<TourDto> UpdateTourAsync(int id, UpdateTourDto dto)
        {
            var tourToUpdate = await _tourRepository.GetTourByIdAsync(id);

            if (tourToUpdate == null)
                return new TourDto();

            _mapper.Map(dto, tourToUpdate);

            await _tourRepository.UpdateTourAsync(tourToUpdate);

            var resultDto = _mapper.Map<TourDto>(tourToUpdate);

            return resultDto;
        }
    }
}
