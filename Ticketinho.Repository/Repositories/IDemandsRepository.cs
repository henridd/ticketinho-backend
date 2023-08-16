using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
    public interface IDemandsRepository : IRepositoryBase<Demand>
    {
        Task DeactivateDemandsAsync(DateTime utcNow);
        Task<IEnumerable<Demand>> GetAllActiveAsync();
        Task ReactivateAsync(Demand ticket);
    }
}
