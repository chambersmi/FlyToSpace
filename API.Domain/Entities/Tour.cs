﻿using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Entities
{
    public class Tour
    {
        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public string TourDescription { get; set; } = null!;
        public decimal TourPrice { get; set; }
        public int MaxSeats { get; set; }
        public int SeatsOccupied { get; set; }
        public string? ImageUrl { get; set; }
    }
}
