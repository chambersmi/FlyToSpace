using API.Domain.Enums;
using System.Text.Json.Serialization;

namespace API.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirthFormatted => DateOfBirth.ToString("yyyy-MM-dd");
        [JsonIgnore]
        public DateTime DateOfBirth { get; set; }

        public string StreetAddress1 { get; set; } = null!;
        public string? StreetAddress2 { get; set; }
        public string City { get; set; } = null!;
        public StateEnum State { get; set; }
        public string ZipCode { get; set; } = null!;
        public List<ItineraryDto> Bookings { get; set; } = new List<ItineraryDto>();
    }
}
