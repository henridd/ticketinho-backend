using System.Text.Json.Serialization;

namespace Ticketinho.Common.DTOs.Auth
{
    public class CreateUserRequestDto
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }

        [JsonPropertyName("phoneNumber")]
        public required string PhoneNumber { get; set; }
    }
}

