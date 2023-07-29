using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{

    public class TicketsRepository : RepositoryBase<Ticket>, ITicketsRepository
    {
        protected override string CollectionName => "tickets";
    }
}
