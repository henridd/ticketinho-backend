using Ticketinho.Common.Enums;

namespace Ticketinho.Common.DTOs.Tickets
{
    public class UpdateTicketRequestDto
    {
        public TicketZone Zone { get; set; }

        public TicketType Type { get; set; }

        public double Price { get; set; }
    }
}
