using Ticketinho.Repository.Transactions.Operations;

namespace Ticketinho.Repository.Transactions
{
    public class UnitOfWork
    {
        internal Queue<BaseTransactionOperation> Operations { get; } = new();

        public UnitOfWork WithUpdate(string collectionName, string id, string propertyName, object value)
        {
            Operations.Enqueue(new UpdateTransactionOperation(collectionName, id, propertyName, value));

            return this;
        }

        public UnitOfWork WithCreate(string collectionName, object data)
        {
            Operations.Enqueue(new CreateTransactionOperation(collectionName, data));

            return this;
        }
    }
}
