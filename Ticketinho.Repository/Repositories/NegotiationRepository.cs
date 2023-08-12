using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
    public class NegotiationRepository : RepositoryBase<Negotiation>, INegotiationsRepository
    {
        protected override string CollectionName => RepositoryNames.Negotiations;

        public async Task<bool> OpenNegotiationExistsAsync(string ticketId)
        {
            var tickets = await GetByPropertyAsync("TicketId", ticketId);

            return tickets.Any(x => x.IsActive);
        }
    }
}
