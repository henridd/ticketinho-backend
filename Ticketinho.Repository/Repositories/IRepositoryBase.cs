namespace Ticketinho.Repository.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task AddAsync(T model);

        Task<T?> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAllAsync();

        Task DeleteAsync(string id);

        Task UpdateAsync(T model);

        Task SaveOrUpdateAsync(T model);
    }
}
