using System.Security.Claims;
using Ticketinho.Repository.Models;

namespace Ticketinho.Service.Auth
{
    public interface IJwtBuilder
    {
        string Build(string iss, string aud);
        IJwtBuilder WithExpiration(DateTime expiration);
        IJwtBuilder WithClaims(IList<Claim> claims);
    }
}