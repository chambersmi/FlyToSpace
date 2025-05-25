using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class CreateBookingDto
    {
        public int TourId { get; set; }
        public string UserId { get; set; } = null!;
        public int? FlightId { get; set; }
        public int? CustomerId { get; set; }
        public string? SeatNumber { get; set; }
        public string? Status { get; set; }
        public int DurationInDays { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? BookingDate { get; set; }
    }

}
