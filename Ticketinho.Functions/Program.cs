using Microsoft.Extensions.Hosting;
using Ticketinho.Middlewares;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<ExceptionHandlingMiddleware>();
    })
    .Build();

host.Run();
