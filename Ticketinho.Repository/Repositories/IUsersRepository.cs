using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
    public interface IUsersRepository : IRepositoryBase<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}