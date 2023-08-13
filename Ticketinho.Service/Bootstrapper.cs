using Microsoft.Extensions.DependencyInjection;
using Ticketinho.Service.Auth;
using Ticketinho.Service.Negotiations;
using Ticketinho.Service.Tickets;

namespace Ticketinho.Service
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            RegisterApplicationServices(services);

            services.AddScoped<IJwtBuilder, JwtBuilder>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<INegotiationService, NegotiationService>();
        }
    }
}
