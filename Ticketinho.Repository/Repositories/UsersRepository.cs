using System;
using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
	public class UsersRepository : RepositoryBase<User>, IUsersRepository
	{
		protected override string CollectionName => "users";
    }
}

