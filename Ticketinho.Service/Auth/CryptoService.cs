using System.Security.Cryptography;
using System.Text;

namespace Ticketinho.Service.Auth
{
	public class CryptoService : ICryptoService
	{
        const int KeySize = 64;
        const int Iterations = 350000;
        private readonly HashAlgorithmName _algorithmName = HashAlgorithmName.SHA512;

        public string HashPassword(string password, out byte[] salt)
		{
            salt = RandomNumberGenerator.GetBytes(KeySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    Iterations,
                    _algorithmName,
                    KeySize
                );

            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _algorithmName, KeySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}

