using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ticketinho.Common.DTOs;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;

namespace Ticketinho.Service.Auth
{
	public class AuthService : IAuthService
	{
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtBuilder _jwtBuilder;

        public AuthService(IUsersRepository usersRepository, IJwtBuilder jwtBuilder)
		{
            _usersRepository = usersRepository;
            _jwtBuilder = jwtBuilder;
		}

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _usersRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginResponseDto();
            }

            // TODO: We should store the hashed password
            if (request.Password != user.Password)
            {
                return new LoginResponseDto() { Email = user.Email };
            }

            var token = _jwtBuilder.WithExpiration(DateTime.UtcNow.AddMinutes(15))
                .WithClaims(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "TicketinhoToken"),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email)
                }).Build(iss: "TicketinhoBackend", aud: "TicketinhoBackend");

            return new LoginResponseDto { Email = user.Email, Token = token };
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

