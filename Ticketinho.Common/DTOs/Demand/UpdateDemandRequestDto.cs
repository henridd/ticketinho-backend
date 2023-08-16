using System.Text.Json.Serialization;
using Ticketinho.Common.Enums;

namespace Ticketinho.Common.DTOs.Demand
{
    public class UpdateDemandRequestDto
    {
        public TicketZone Zone { get; set; }

        public TicketType Type { get; set; }

        public double Price { get; set; }
    }
}
