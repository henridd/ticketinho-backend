using Microsoft.Extensions.DependencyInjection;

namespace Ticketinho.Bootstrap
{
    public static class Bootstrapper
    {
        public static void Start(IServiceCollection services)
        {
            Ticketinho.Repository.Bootstrapper.RegisterServices(services);
            Ticketinho.Service.Bootstrapper.RegisterServices(services);
        }
    }
}
