using System;
using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
	public class UsersRepository : RepositoryBase<User>, IUsersRepository
	{
		protected override string CollectionName => RepositoryNames.Users;

        public async Task<User?> GetByEmailAsync(string email)
        {
            return (await GetByPropertyAsync("Email", email)).FirstOrDefault();
        }
    }
}

