using System;
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error processing invocation");

                var httpRequestData = await context.GetHttpRequestDataAsync();

                if(httpRequestData != null)
                {
                    var newResponse = httpRequestData.CreateResponse(HttpStatusCode.InternalServerError);
                    await newResponse.WriteAsJsonAsync(new { Error = "Internal Server Error" }, newResponse.StatusCode);
                    var invocationResult = context.GetInvocationResult();

                    var httpOutputBinding = context.GetOutputBindings<HttpResponseData>()
                        .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");
                    if(httpOutputBinding is not null)
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
    }
}

