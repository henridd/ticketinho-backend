namespace Ticketinho.Service.Auth
{
	public interface ICryptoService
	{
        string HashPassword(string password, out byte[] salt);
        bool VerifyPassword(string password, string hash, byte[] salt);
    }
}

