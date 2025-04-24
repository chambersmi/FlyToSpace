using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domains.DTOs
{
    public class UserRequestDto
    {        
        public string? FirstName { get; set; } 
        
        public string? LastName { get; set; } 
        
        public DateTime Birthdate { get; set; }
        
        public string? Email { get; set; } 
        
        public string? PhoneNumber { get; set; }
        
        public string? City { get; set; }
        
        public string? State { get; set; }
        
        public string? ZipCode { get; set; }
    }
}
