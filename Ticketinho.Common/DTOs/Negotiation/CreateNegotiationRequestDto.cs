using Ticketinho.Common.Enums;

namespace Ticketinho.Common.DTOs.Negotiation
{
    public class CreateNegotiationRequestDto
    {
        public required string TicketId { get; set; }

        public required string SellerId { get; set; }

        public required string BuyerId { get; set; }
    }
}
