using Microsoft.Extensions.DependencyInjection;
using Ticketinho.Repository.Repositories;
using Ticketinho.Repository.Transactions;

namespace Ticketinho.Repository
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWorkRunner, UnitOfWorkRunner>();

            RegisterRepositories(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<ITicketsRepository, TicketsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<INegotiationsRepository, NegotiationRepository>();
            services.AddScoped<IDemandsRepository, DemandsRepository>();
        }
    }
}
