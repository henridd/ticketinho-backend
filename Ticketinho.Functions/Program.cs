using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Ticketinho.Bootstrap;
using Ticketinho.Middlewares;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<ExceptionHandlingMiddleware>();
    })
    .ConfigureServices(services =>
    {
        Bootstrapper.Start(services);

        services.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

    })
    .Build();

host.Run();
