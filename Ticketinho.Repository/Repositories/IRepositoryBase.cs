namespace Ticketinho.Repository.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task AddAsync(T model);
    }
}
