using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
    public interface INegotiationsRepository : IRepositoryBase<Negotiation>
    {
        Task<bool> OpenNegotiationExistsAsync(string ticketId);
    }
}
