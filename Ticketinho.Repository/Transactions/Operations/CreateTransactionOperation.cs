namespace Ticketinho.Repository.Transactions.Operations
{
    internal class CreateTransactionOperation : BaseTransactionOperation
    {
        public override OperationType OperationType => OperationType.Create;

        public object Data { get; }

        public CreateTransactionOperation(string collectionName, object data)
            : base(collectionName)
        {
            Data = data;
        }
    }

}
