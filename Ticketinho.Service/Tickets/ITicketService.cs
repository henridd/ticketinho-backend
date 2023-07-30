using Ticketinho.Common.Enums;
using Ticketinho.Repository.Models;

namespace Ticketinho.Service.Tickets
{
    public interface ITicketService
    {
        Task<string> AddAsync(string ownerId, TicketZone zone, TicketType type, double price);

        Task<Ticket?> GetAsync(string id);

        Task<IEnumerable<Ticket>> GetAllAsync();

        Task UpdateAsync(string id, TicketZone zone, TicketType type, double price);

        Task DeleteAsync(string ticketId);

        Task ReactivateTicketAsync(string id);

        Task DeactivateOldTicketsAsync();
    }
}
