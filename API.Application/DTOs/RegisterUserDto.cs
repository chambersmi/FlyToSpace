using API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class RegisterUserDto
    {
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public string StreetAddress1 { get; set; } = null!;
        public string? StreetAddress2 { get; set; }
        public string City { get; set; } = null!;
        public StateEnum State { get; set; }
        public string ZipCode { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
