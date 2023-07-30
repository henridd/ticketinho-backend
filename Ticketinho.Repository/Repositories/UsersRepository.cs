﻿using System;
using Ticketinho.Repository.Models;

namespace Ticketinho.Repository.Repositories
{
	public class UsersRepository : RepositoryBase<User>, IUsersRepository
	{
		protected override string CollectionName => "users";

        public async Task<User?> GetByEmailAsync(string email)
        {
            var query = Collection.WhereEqualTo("Email", email);
            var snapshot = await query.GetSnapshotAsync();
            
            return snapshot.Documents.FirstOrDefault()?.ConvertTo<User>();
        }
    }
}
