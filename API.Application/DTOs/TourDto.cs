using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class TourDto
    {
        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public string TourDescription { get; set; } = null!;
        public decimal TourPackagePrice { get; set; }
        public int MaxSeats { get; set; }
        public int SeatsOccupied { get; set; }
    }
}
