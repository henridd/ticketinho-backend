using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
    public class DemandsRepository : RepositoryBase<Demand>, IDemandsRepository
    {
        protected override string CollectionName => RepositoryNames.Demands;

        public async Task<IEnumerable<Demand>> GetAllActiveAsync()
        {
            var query = Collection.WhereEqualTo("IsActive", true);

            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Select(x => x.ConvertTo<Demand>());
        }

        public async Task DeactivateDemandsAsync(DateTime maximumValidDate)
        {
            var query = Collection.WhereLessThan("CreatedAt", maximumValidDate);

            var update = new Dictionary<string, object>()
            {
                { "IsActive", false }
            };

            var snapshot = await query.GetSnapshotAsync();

            var batch = Database.StartBatch();

            foreach (var document in snapshot.Documents)
            {
                batch.Update(document.Reference, update);
            }

            await batch.CommitAsync();
        }

        public async Task ReactivateAsync(Demand demand)
            => await UpdateSinglePropertyAsync(demand.Id, nameof(demand.IsActive), true);
    }
}
