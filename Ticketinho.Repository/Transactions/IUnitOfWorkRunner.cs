namespace Ticketinho.Repository.Transactions
{
    public interface IUnitOfWorkRunner
    {
        Task<IEnumerable<string>> RunAsync(UnitOfWork uow);
    }
}
