using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int TourId { get; set; }
        public Tour Tour { get; set; } = null!;

        public string UserId { get; set; } = null!;

        //public int? FlightId { get; set; }
        //public Flight? Flight { get; set; }

        public int? CustomerId { get; set; }
        public ApplicationUser? Customer { get; set; }

        public string? SeatNumber { get; set; }

        public string? Status { get; set; }

        public int DurationInDays { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    }

}
