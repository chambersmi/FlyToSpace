﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class ItineraryDto
    {
        public int ItineraryId { get; set; }

        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; } = null!;
        public int SeatsBooked { get; set; }

        public DateTime BookingDate { get; set; }
        public int? FlightId { get; set; } 
    }
}
