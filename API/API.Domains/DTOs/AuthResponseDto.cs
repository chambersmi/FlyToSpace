using API.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domains.DTOs
{
    public class AuthResponseDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public bool isSuccessful { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public static AuthResponseDto Success(ApplicationUser user, string? token = null)
        {
            return new AuthResponseDto
            {
                isSuccessful = true,
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static AuthResponseDto Failure(params string[] errors)
        {
            return new AuthResponseDto
            {
                isSuccessful = false,
                Errors = errors
            };
        }

        public static AuthResponseDto Failure(IEnumerable<string> errors)
        {
            return new AuthResponseDto
            {
                isSuccessful = false,
                Errors = errors
            };
        }
    }
}
