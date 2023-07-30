using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NUnit.Framework;
using Ticketinho.Repository.Models;
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

            var user = new User()
            {
                Id = "1",
                Email = "luke@skywalker.com",
                Password = "yoda123"
            };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TicketinhoToken"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email)
            };

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

            // Trim milli and micro seconds
            expiration = expiration.AddMilliseconds(-expiration.Millisecond);
            expiration = expiration.AddMicroseconds(-expiration.Microsecond);

            // Act
            var token = new JwtBuilder().WithExpiration(expiration)
                .Build(iss: "TicketinhoBackend", aud: "TicketinhoBackend");

            // Assert
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var exp = jwt.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value;
            var expireDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;
            
            Assert.That(expireDateTime, Is.EqualTo(expiration));
        }
	}
}

