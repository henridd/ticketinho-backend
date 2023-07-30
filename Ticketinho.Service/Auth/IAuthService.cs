using Ticketinho.Common.DTOs.Auth;

namespace Ticketinho.Service.Auth
{
        public interface IAuthService
        {
                Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
                Task RegisterUserAsync(CreateUserRequestDto request);
        }
}