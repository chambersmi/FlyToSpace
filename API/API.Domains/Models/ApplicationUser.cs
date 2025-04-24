
using Microsoft.AspNetCore.Identity;

namespace API.Domains.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Inherited
        //public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime Birthdate { get; set; }
        
        public required string City { get; set; }

        //Make SelectList
        public required string State { get; set; }
        public required string ZipCode { get; set; }

    }
}
