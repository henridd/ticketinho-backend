namespace Ticketinho.Repository.Transactions.Operations
{
    internal abstract class BaseTransactionOperation
    {
        public string CollectionName { get; }

        public abstract OperationType OperationType { get; }

        public BaseTransactionOperation(string collectionName)
        {
            CollectionName = collectionName;
        }
    }

}
