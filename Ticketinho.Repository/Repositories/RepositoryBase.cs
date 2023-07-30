using Google.Cloud.Firestore;
using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : Model
    {
        protected FirestoreDb Database { get; private set; }

        protected abstract string CollectionName { get; }

        protected CollectionReference Collection => Database.Collection(CollectionName);

        public RepositoryBase()
        {
            using StreamReader r = new StreamReader("secrets.json");
            string json = r.ReadToEnd();

            Database = new FirestoreDbBuilder
            {
                ProjectId = "ticketinho",
                JsonCredentials = json
            }.Build();
        }

        public virtual async Task<string> AddAsync(T model)
        {
            var newDocument = await Collection.AddAsync(model);

            return newDocument.Id;
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            var document = Collection.Document(id);
            if(document == null)
            {
                return null;
            }

            var snapshot = await document.GetSnapshotAsync();

            return snapshot.ConvertTo<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var snapshot = await Collection.GetSnapshotAsync();

            return snapshot.Select(x => x.ConvertTo<T>());
        }

        public virtual async Task DeleteAsync(string id)
        {
            await Collection.Document(id).DeleteAsync();
        }

        public virtual async Task UpdateAsync(T model)
        {
            var document = Collection.Document(model.Id);
            var snapshot = await document.GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                return;
            }

            await document.SetAsync(model);
        }

        public virtual async Task SaveOrUpdateAsync(T model)
        {
            await Collection.Document(model.Id).SetAsync(model);
        }
    }
}
