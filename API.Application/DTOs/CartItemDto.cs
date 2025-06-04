using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class CartItemDto
    {
        public Guid BookingId { get; set; } = Guid.NewGuid();
        public int TourId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public int SeatsBooked { get; set; }
        public decimal TourPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
