using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ticketinho.Service.Auth
{
	public class JwtBuilder : IJwtBuilder
	{
		private DateTime? _expiration = null;
		private List<Claim> _claims = new();

		public string Build(string iss, string aud)
		{
			var token = new JwtSecurityToken(
				issuer: iss,
				audience: aud,
				claims: _claims,
				signingCredentials: CreateSigningCredentials(),
				expires: _expiration
				);
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}

		public IJwtBuilder WithExpiration(DateTime expiration)
		{
			_expiration = expiration;
			return this;
		}

		public IJwtBuilder WithClaims(IList<Claim> claims)
		{
			_claims.AddRange(claims);
			return this;
		}

        // TODO: Should get the secret from env environment.
        private static SigningCredentials CreateSigningCredentials()
		{
            return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("!TicketinhoSecret!")
            ),
            SecurityAlgorithms.HmacSha256
        );
        }
	}
}

