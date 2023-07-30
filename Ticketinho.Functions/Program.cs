using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ticketinho.Middlewares;
using Ticketinho.Repository.Repositories;
using Ticketinho.Service.Auth;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<ExceptionHandlingMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddScoped<ITicketsRepository, TicketsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        services.AddScoped<IJwtBuilder, JwtBuilder>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICryptoService, CryptoService>();
    })
    .Build();

host.Run();
