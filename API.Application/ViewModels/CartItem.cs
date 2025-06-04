using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.ViewModels
{
    public class CartItem
    {
        public int CartId { get; set; }
        public string? TourName { get; set; }
        public int Seats { get; set; }
        public decimal Price { get; set; }
    }
}
