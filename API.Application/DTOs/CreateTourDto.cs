using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class CreateTourDto
    {
        public string TourName { get; set; } = null!;
        public string TourDescription { get; set; } = null!;
        public decimal TourPricePerDay { get; set; }
        public int MaxSeats { get; set; }
    }
}
