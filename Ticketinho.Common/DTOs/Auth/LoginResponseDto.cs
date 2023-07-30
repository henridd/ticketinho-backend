using System.Text.Json.Serialization;

namespace Ticketinho.Common.DTOs.Auth
{
	public class LoginResponseDto
	{
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }
		
	}
}

