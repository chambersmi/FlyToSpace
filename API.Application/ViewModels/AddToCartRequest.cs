using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.ViewModels
{
    public class AddToCartRequest
    {
        public int TourId { get; set; }
        public int SeatsBooked { get; set; }
    }
}
