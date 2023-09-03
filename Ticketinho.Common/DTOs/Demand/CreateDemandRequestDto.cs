using System.Text.Json.Serialization;
using Ticketinho.Common.Enums;

namespace Ticketinho.Common.DTOs.Demand
{
    public class CreateDemandRequestDto
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("zone")]
        public TicketZone Zone { get; set; }

        [JsonPropertyName("type")]
        public TicketType Type { get; set; }
    }
}
