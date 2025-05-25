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
        public ApplicationUser? User { get; set; } = null!;        

        public int? FlightId { get; set; }
        public int SeatsBooked { get; set; } // can change this variable name ot something better.
        public string? Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    }

}
