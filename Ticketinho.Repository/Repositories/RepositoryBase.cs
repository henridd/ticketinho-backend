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

        public virtual async Task AddAsync(T model)
        {
            await Collection.AddAsync(model);
        }
    }
}
