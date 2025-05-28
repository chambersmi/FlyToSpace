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
    public class BookingService : IBookingService
    {
        private ILogger<BookingService> _logger;
        private IBookingRepository _bookingRepository;
        private ITourRepository _tourRepository;
        private readonly IMapper _mapper;

        public BookingService(
            ILogger<BookingService> logger,
            IBookingRepository bookingRepository,
            ITourRepository tourRepository,
            IMapper mapper
            )
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public async Task<BookingDto?> CreateBookingAsync(CreateBookingDto dto)
        {            
            var bookingEntity = _mapper.Map<Booking>(dto);

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

            var booking = await _bookingRepository.CreateBookingAsync(bookingEntity);
            var resultDto = _mapper.Map<BookingDto>(booking);

            return resultDto;
        }

        public async Task<BookingDto?> UpdateBookingAsync(int bookingId, UpdateBookingDto dto)
        {
            var existingBooking = await _bookingRepository.GetBookingByIdAsync(bookingId);

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

            await _bookingRepository.UpdateBookingAsync(existingBooking);

            var resultDto = _mapper.Map<BookingDto>(existingBooking);

            return resultDto;
        }

        public async Task<bool> DeleteBookingByIdAsync(int id)
        {
            var bookingToDelete = await _bookingRepository.GetBookingByIdAsync(id);
            if (bookingToDelete == null)
                return false;

            await _bookingRepository.DeleteBookingAsync(bookingToDelete.BookingId);

            return true;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            return _mapper.Map<BookingDto>(booking);
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

            return (tour.TourPackagePrice * seatsBooked) * (1 + taxAmount);
        }

        private async Task UpdateSeatAvailabilityAsync(int tourId, int seatsToBook, int? originalSeatsBooked = null)
        {

        }
    }
}
