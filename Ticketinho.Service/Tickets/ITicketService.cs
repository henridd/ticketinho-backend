using Ticketinho.Repository.Models;
using Ticketinho.Repository.Models.Enums;

namespace Ticketinho.Service.Tickets
{
    public interface ITicketService
    {
        Task<string> AddAsync(string ownerId, TicketZone zone, TicketType type, double price);

        Task<Ticket?> GetAsync(string id);

        Task<IEnumerable<Ticket>> GetAllAsync();

        Task UpdateAsync(string id, TicketZone zone, TicketType type, double price);

        Task DeleteAsync(string ticketId);
    }
}
