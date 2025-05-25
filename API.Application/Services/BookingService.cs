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
        private readonly IMapper _mapper;

        public BookingService(
            ILogger<BookingService> logger,
            IBookingRepository bookingRepository,
            IMapper mapper
            )
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingDto?> CreateBookingAsync(CreateBookingDto dto)
        {
            var bookingEntity = _mapper.Map<Booking>(dto);

            if(!bookingEntity.FlightId.HasValue)
            {
                var random = new Random();
                bookingEntity.FlightId = random.Next(1, 1000);
            }
            
            var booking = await _bookingRepository.CreateBookingAsync(bookingEntity);
            var resultDto = _mapper.Map<BookingDto>(booking);

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

        public async Task<BookingDto?> UpdateBookingAsync(int id, UpdateBookingDto dto)
        {
            var bookingToUpdate = await _bookingRepository.GetBookingByIdAsync(id);

            if (bookingToUpdate == null)
                return new BookingDto();

            _mapper.Map(dto, bookingToUpdate);

            await _bookingRepository.UpdateBookingAsync(bookingToUpdate);

            var resultDto = _mapper.Map<BookingDto>(bookingToUpdate);

            return resultDto;
        }
    }
}
