using System.Text.Json.Serialization;
using Ticketinho.Common.Enums;

namespace Ticketinho.Common.DTOs.Negotiation
{
    public class CreateNegotiationRequestDto
    {
        [JsonPropertyName("ticketId")]
        public required string TicketId { get; set; }

        [JsonPropertyName("sellerId")]
        public required string SellerId { get; set; }

        [JsonPropertyName("buyerId")]
        public required string BuyerId { get; set; }
    }
}
