using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
    public interface ITicketsRepository : IRepositoryBase<Ticket>
    {
        Task ReactivateAsync(Ticket ticket);

        Task DeactivateTicketsAsync(DateTime maximumValidDate);

        Task<IEnumerable<Ticket>> GetAllActiveAsync();
    }
}
