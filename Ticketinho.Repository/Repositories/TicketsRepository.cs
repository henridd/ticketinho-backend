using Google.Cloud.Firestore;
using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{

    public class TicketsRepository : RepositoryBase<Ticket>, ITicketsRepository
    {
        protected override string CollectionName => "tickets";

        public async Task ReactivateAsync(Ticket ticket)
        {
            var document = Collection.Document(ticket.Id);

            await document.UpdateAsync("IsActive", true);
        }
    }
}
