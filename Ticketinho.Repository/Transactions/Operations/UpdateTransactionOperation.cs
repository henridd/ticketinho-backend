namespace Ticketinho.Repository.Transactions.Operations
{
    internal class UpdateTransactionOperation : BaseTransactionOperation
    {
        public override OperationType OperationType => OperationType.Update;

        public string DocumentId { get; }
        public string PropertyName { get; }
        public object Value { get; }

        public UpdateTransactionOperation(string collectionName, string id, string propertyName, object value)
            : base(collectionName)
        {
            DocumentId = id;
            PropertyName = propertyName;
            Value = value;
        }
    }

}
