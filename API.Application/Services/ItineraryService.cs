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
    public class ItineraryService : IItineraryService
    {
        private ILogger<ItineraryService> _logger;
        private IItineraryRepository _bookingRepository;
        private ITourRepository _tourRepository;
        private readonly IMapper _mapper;

        public ItineraryService(
            ILogger<ItineraryService> logger,
            IItineraryRepository bookingRepository,
            ITourRepository tourRepository,
            IMapper mapper
            )
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public async Task<ItineraryDto?> CreateItineraryAsync(CreateItineraryDto dto)
        {            
            var bookingEntity = _mapper.Map<Itinerary>(dto);

            // Assign random Flight Id
            // If a user is booking multiple seats, it'll have to be the same id
            if (!bookingEntity.FlightId.HasValue)
            {
                var random = new Random();
                bookingEntity.FlightId = random.Next(1, 1000);
            }

            if (dto.SeatsBooked <= 0)
            {
                throw new Exception("Must book at least one seat.");
            }

            // Check seat availability
            // Tours has MaxSeats and SeatsOccupied
            var tourEntity = await _tourRepository.GetTourByIdAsync(dto.TourId);
            var maxSeats = tourEntity.MaxSeats;
            var seatsOccupied = tourEntity.SeatsOccupied;
            
            // Subtract max seats from seats occupied
            var availableSeats = maxSeats - seatsOccupied;

            if(dto.SeatsBooked > availableSeats)
            {
                throw new Exception("There are not enough seats.");                
            }

            tourEntity.SeatsOccupied += dto.SeatsBooked;
            await _tourRepository.UpdateTourAsync(tourEntity);
            // End check seat availablity

            bookingEntity.BookingDate = DateTime.UtcNow;
            bookingEntity.TotalPrice = await CalculuateTotalPriceAsync(dto.TourId, dto.SeatsBooked);

            var booking = await _bookingRepository.CreateItineraryAsync(bookingEntity);
            var resultDto = _mapper.Map<ItineraryDto>(booking);

            return resultDto;
        }

        public async Task<ItineraryDto?> UpdateItineraryAsync(int bookingId, UpdateItineraryDto dto)
        {
            var existingBooking = await _bookingRepository.GetItineraryByIdAsync(bookingId);

            if (existingBooking == null)
                return null;

            if (dto.SeatsBooked <= 0)
                throw new Exception("Must book at least one seat.");
            
            // Check seat availability
            // Tours has MaxSeats and SeatsOccupied
            var tourEntity = await _tourRepository.GetTourByIdAsync(existingBooking.TourId);
            var maxSeats = tourEntity.MaxSeats;
            var seatsOccupied = tourEntity.SeatsOccupied;

            // Subtract max seats from seats occupied
            var availableSeats = maxSeats - seatsOccupied;

            if (dto.SeatsBooked > availableSeats)
            {
                throw new Exception("There are not enough seats.");
            }

            tourEntity.SeatsOccupied += dto.SeatsBooked;
            await _tourRepository.UpdateTourAsync(tourEntity);
            // End check seat availablity

            existingBooking.TotalPrice = await CalculuateTotalPriceAsync(existingBooking.TourId, dto.SeatsBooked);

            // FlightId should change if they are changing the date            
            existingBooking.FlightId = dto.FlightId ?? existingBooking.FlightId;
            existingBooking.BookingDate = dto.BookingDate ?? existingBooking.BookingDate;

            await _bookingRepository.UpdateItineraryAsync(existingBooking);

            var resultDto = _mapper.Map<ItineraryDto>(existingBooking);

            return resultDto;
        }

        public async Task<bool> DeleteItineraryByIdAsync(int id)
        {
            var bookingToDelete = await _bookingRepository.GetItineraryByIdAsync(id);
            if (bookingToDelete == null)
                return false;

            await _bookingRepository.DeleteItineraryByIdAsync(bookingToDelete.ItineraryId);

            return true;
        }

        /// <summary>
        /// Returns all bookings made by that user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ItineraryDto>> GetAllItinerariesByUserIdAsync(string userId)
        {
            var bookings = await _bookingRepository.GetAllItinerariesByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ItineraryDto>>(bookings);
        }

        /// <summary>
        /// Gets a single booking by user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ItineraryDto?> GetSingleUserItineraryByIdAsync(int id, string userId)
        {
            var booking = await _bookingRepository.GetSingleUserItineraryByIdAsync(id, userId);
            
            if(booking == null || booking.UserId != userId)
            {
                return null;
            }

            return _mapper.Map<ItineraryDto>(booking);
        }

        // Update with Seats Booked
        private async Task<decimal> CalculuateTotalPriceAsync(int tourId, int seatsBooked)
        {
            decimal taxAmount = 0.06m;

            var tour = await _tourRepository.GetTourByIdAsync(tourId);

            if (tour == null)
            {
                throw new Exception($"{tourId} not found.");
            }

            return (tour.TourPrice * seatsBooked) * (1 + taxAmount);
        }

        public async Task<IEnumerable<ItineraryDto>> GetAllItinerariesAsync()
        {
            var booking = await _bookingRepository.GetAllItinerariesAsync();

            if (booking == null)
            {
                throw new Exception($"No itineraries were found.");
            }

            return _mapper.Map<IEnumerable<ItineraryDto>>(booking);
        }

        public async Task<ItineraryDto> GetItineraryByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetItineraryByIdAsync(id);

            if (booking == null)
            {
                throw new Exception($"Itinerary with {id} was not found.");
            }

            return _mapper.Map<ItineraryDto>(booking);
        }

        public async Task<decimal> GetTotalPriceAsync(int id)
        {
            var booking = await _bookingRepository.GetItineraryByIdAsync(id);
            
            var totalPrice = booking.TotalPrice;

            return totalPrice;

        }
    }
}
