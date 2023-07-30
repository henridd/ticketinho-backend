using System.Text.Json.Serialization;
using Ticketinho.Common.Enums;

namespace Ticketinho.Common.DTOs.Tickets
{
    public class UpdateTicketRequestDto
    {
        [JsonPropertyName("zone")]
        public TicketZone Zone { get; set; }

        [JsonPropertyName("type")]
        public TicketType Type { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }
    }
}
