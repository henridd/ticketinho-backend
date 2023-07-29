using System.IdentityModel.Tokens.Jwt;
using Ticketinho.Common.DTOs;

namespace Ticketinho.Service.Auth
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> LoginAsync(string email);
        Task RegisterUserAsync(CreateUserRequestDto request);
    }
}