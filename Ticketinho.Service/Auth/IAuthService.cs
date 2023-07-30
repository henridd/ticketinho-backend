using Ticketinho.Common.DTOs;

namespace Ticketinho.Service.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task RegisterUserAsync(CreateUserRequestDto request);
    }
}