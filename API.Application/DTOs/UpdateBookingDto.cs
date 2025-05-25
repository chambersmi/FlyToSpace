using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class UpdateBookingDto
    {
        public int BookingId { get; set; } 
        public int? FlightId { get; set; }  
        public int? CustomerId { get; set; }
        public string? SeatNumber { get; set; }
        public DateTime? BookingDate { get; set; } 
    }
}
