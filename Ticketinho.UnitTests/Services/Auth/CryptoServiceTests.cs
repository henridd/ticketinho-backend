using NUnit.Framework;
using Ticketinho.Service.Auth;

namespace Ticketinho.UnitTests.Services.Auth
{
	public class CryptoServiceTests
	{
		private readonly ICryptoService _cryptoService = new CryptoService();
		[Test]
		public void ShouldHashPasswordWithSalt()
		{
            // Act
            var hash = _cryptoService.HashPassword("secret_password", out var salt);

			// Arrange
			Assert.That(hash, Is.Not.Null);
			Assert.That(salt, Is.Not.Null);
		}

		[TestCase("secret_password!@/%", "secret_password!@/%", true)]
        [TestCase("secret_password!@/%", "wrong_pass", false)]
        public void ShouldVerifyHashedPassword(string password, string guess, bool expect)
		{
            // Arrange
            var hash = _cryptoService.HashPassword(password, out var salt);

			// Act
			var result = _cryptoService.VerifyPassword(guess, hash, salt);

			// Assert
			Assert.That(result, Is.EqualTo(expect));

        }
	}
}

