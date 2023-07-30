using System.Text.Json.Serialization;

namespace Ticketinho.Common.DTOs.Auth
{
    public class LoginRequestDto
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }
}