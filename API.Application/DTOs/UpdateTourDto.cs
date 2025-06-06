using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class UpdateTourDto
    {
        public string TourName { get; set; } = null!;
        public string TourDescription { get; set; } = null!;
        public decimal TourPrice { get; set; }
        public int MaxSeats { get; set; }
        public int SeatsOccupied { get; set; }
        public string? ImageUrl { get; set; }
    }
}
