using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class CreateItineraryDto
    {
        public int TourId { get; set; }
        public string UserId { get; set; } = null!;
        public int SeatsBooked { get; set; }        
    }
}
