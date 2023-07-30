using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ticketinho.Middlewares;
using Ticketinho.Repository.Repositories;
using Ticketinho.Service.Auth;
using Ticketinho.Service.Tickets;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<ExceptionHandlingMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddScoped<ITicketsRepository, TicketsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITicketService, TicketService>();
    })
    .Build();

host.Run();
