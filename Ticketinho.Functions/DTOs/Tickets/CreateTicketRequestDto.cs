using Ticketinho.Repository.Models.Enums;

namespace Ticketinho.DTOs.Tickets
{
    public class CreateTicketRequestDto
    {
        public required string OwnerId { get; set; }
        public double Price { get; set; }
        public TicketZone Zone { get; set; }
        public TicketType Type { get; set; }
    }
}
