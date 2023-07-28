using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;


namespace Ticketinho.Middlewares
{
	internal sealed class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

		public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                // Code before function execution here
                await next(context);
                // Code after function execution here
            }
            // Currently Azure Functions throw an aggregate exception which contains the exception.
            // https://github.com/Azure/azure-functions-dotnet-worker/issues/993
            catch (AggregateException ex) when (ex.InnerException is HttpRequestException)
            {
                await ProcessException(new { Error = ex.InnerException.Message }, HttpStatusCode.BadRequest, context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error processing invocation");
                await ProcessException(new { Error = "Internal Server Error" }, HttpStatusCode.InternalServerError, context); 
            }
        }

        private async Task ProcessException(object payload, HttpStatusCode statusCode, FunctionContext context)
        {

            var httpRequestData = await context.GetHttpRequestDataAsync();

            if (httpRequestData == null)
            {
                return;
            }

            var newResponse = await httpRequestData.CreateErrorResponseAsync(payload, statusCode);

            var invocationResult = context.GetInvocationResult();
            var httpOutputBinding = context.GetOutputBindings<HttpResponseData>()
                .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");
            if (httpOutputBinding is not null)
            {
                httpOutputBinding.Value = newResponse;
            }
            else
            {
                invocationResult.Value = newResponse;
            }
        }
    }
}

