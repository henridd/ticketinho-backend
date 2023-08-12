using Google.Cloud.Firestore;
using Ticketinho.Repository.Transactions.Operations;

namespace Ticketinho.Repository.Transactions
{
    public class UnitOfWorkRunner : IUnitOfWorkRunner
    {
        private FirestoreDb Database;

        public UnitOfWorkRunner()
        {
            Database = DatabaseConnector.ConnectToDatabase();
        }

        public async Task<IEnumerable<string>> RunAsync(UnitOfWork uow)
        {
            var createdIds = new List<string>();

            await Database.RunTransactionAsync(async transaction =>
            {
                while (uow.Operations.Count > 0)
                {
                    var operation = uow.Operations.Dequeue();
                    switch (operation.OperationType)
                    {
                        case OperationType.None:
                            break;
                        case OperationType.Create:
                            var createdId = RunCreate(transaction, (CreateTransactionOperation)operation);
                            createdIds.Add(createdId);
                            break;
                        case OperationType.Update:
                            RunUpdate(transaction, (UpdateTransactionOperation)operation);
                            break;
                    }
                }
            });

            return createdIds;
        }

        private void RunUpdate(Transaction transaction, UpdateTransactionOperation operation)
        {
            var collection = Database.Collection(operation.CollectionName);
            var documentReference = collection.Document(operation.DocumentId);
            transaction.Update(documentReference, operation.PropertyName, operation.Value);
        }

        private string RunCreate(Transaction transaction, CreateTransactionOperation operation)
        {
            var collection = Database.Collection(operation.CollectionName);
            var documentReference = collection.Document();
            transaction.Create(documentReference, operation.Data);
            return documentReference.Id;
        }
    }
}
