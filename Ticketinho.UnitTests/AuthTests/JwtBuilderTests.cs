using System.IdentityModel.Tokens.Jwt;
using NUnit.Framework;
using Ticketinho.Service.Auth;

namespace Ticketinho.UnitTests.AuthTests
{
	public class JwtProviderTests
	{
		[Test]
		public void ShouldBuildToken_WithIssAndAud()
        {
            // Arrange
            const string Iss = "Audience";
            const string Aud = "Issuer";

            // Act
			var token = new JwtBuilder().Build(iss: Iss, aud: Aud);
			
            // Assert
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var iss = jwt.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Iss).Value;
            var aud = jwt.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Aud).Value;
            Assert.Multiple(() =>
            {
                Assert.That(iss, Is.EqualTo(Iss));
                Assert.That(aud, Is.EqualTo(Aud));
            });
        }

        [Test]
        public void ShouldBuildTokenWithExpiration()
        {
            // Arrange
            var expiration = DateTime.UtcNow.AddMinutes(15);

            // Act
            var token = new JwtBuilder().WithExpiration(expiration)
                .Build(iss: "TicketinhoBackend", aud: "TicketinhoBackend");

            // Assert
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var exp = jwt.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value;
            var expireDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime.Ticks;
            Assert.That(expireDateTime / 10000000, Is.EqualTo(expiration.Ticks/ 10000000));
        }
	}
}
