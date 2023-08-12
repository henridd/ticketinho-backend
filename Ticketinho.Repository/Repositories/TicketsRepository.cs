using Google.Cloud.Firestore;
using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{

    public class TicketsRepository : RepositoryBase<Ticket>, ITicketsRepository
    {
        protected override string CollectionName => RepositoryNames.Tickets;

        public async Task ReactivateAsync(Ticket ticket) 
            => await UpdateSinglePropertyAsync(ticket.Id, nameof(ticket.IsActive), true);

        public async Task DeactivateTicketsAsync(DateTime maximumValidDate)
        {
            var query = Collection.WhereLessThan("CreatedAt", maximumValidDate);

            var update = new Dictionary<string, object>()
            {
                { "IsActive", false }
            };

            var snapshot = await query.GetSnapshotAsync();

            var batch = Database.StartBatch();

            foreach(var document in snapshot.Documents)
            {
                batch.Update(document.Reference, update);
            }

            await batch.CommitAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAllActiveAsync()
        {
            var query = Collection.WhereEqualTo("IsActive", true);

            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Select(x => x.ConvertTo<Ticket>());
        }
    }
}
