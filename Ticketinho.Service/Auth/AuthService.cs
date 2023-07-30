using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Ticketinho.Common.DTOs;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;

namespace Ticketinho.Service.Auth
{
	public class AuthService : IAuthService
	{
        private readonly IUsersRepository _usersRepository;

        public AuthService(IUsersRepository usersRepository)
		{
            _usersRepository = usersRepository;
		}

        public async Task<JwtSecurityToken> LoginAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email) ??
                throw new HttpRequestException("User not found", null, HttpStatusCode.NotFound);
            throw new NotImplementedException();
        }

        public async Task RegisterUserAsync(CreateUserRequestDto request)
        {
            await _usersRepository.AddAsync(
                new User(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.PhoneNumber
                    )
                );
        }
    }
}

