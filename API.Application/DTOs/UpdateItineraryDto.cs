﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class UpdateItineraryDto
    {        
        public int? FlightId { get; set; }  
        public int SeatsBooked { get; set; }
        public DateTime? BookingDate { get; set; } 
    }
}
