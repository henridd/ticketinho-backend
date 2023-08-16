using Ticketinho.Common.Enums;
using Ticketinho.Repository.Models;

namespace Ticketinho.Service.Demands
{
    public interface IDemandService
    {
        Task<string> AddAsync(string requesterId, TicketZone zone, TicketType type, double price);
        Task DeactivateOldDemandsAsync();
        Task DeleteAsync(string id);
        Task<IEnumerable<Demand>> GetAllActiveAsync();
        Task<Demand?> GetAsync(string id);
        Task ReactivateDemandAsync(string id);
        Task UpdateAsync(string id, TicketZone zone, TicketType type, double price);
    }
}
