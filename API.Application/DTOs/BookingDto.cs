using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }

        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public decimal TourPricePerDay { get; set; }

        public int DurationInDays { get; set; }
        public decimal TotalPrice { get; set; }

        public string UserId { get; set; } = null!;
        public string? SeatNumber { get; set; }

        public DateTime BookingDate { get; set; }
        public int? FlightId { get; set; } 
    }
}
