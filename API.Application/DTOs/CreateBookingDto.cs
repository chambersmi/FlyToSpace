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

        public int DurationInDays { get; set; }
        //public decimal TotalPrice { get; set; } // Calculate later

        public string? SeatNumber { get; set; }

        public int? FlightId { get; set; }
        public DateTime? BookingDate { get; set; } = DateTime.UtcNow;
    }

}
