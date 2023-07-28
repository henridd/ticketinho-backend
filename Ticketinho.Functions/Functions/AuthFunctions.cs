using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ticketinho.DTOs;
using Ticketinho.DTOs.Validation;

namespace Ticketinho
{
    public class AuthFunctions
    {
        private readonly ILogger _logger;

        public AuthFunctions(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AuthFunctions>();
        }

        [Function("Login")]
        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var loginRequest = await req.GetJsonBody<LoginRequestDto, LoginRequestValidator>();

            if (!loginRequest.IsValid)
            {
                return await loginRequest.ToBadRequest(req);
            }

            _logger.LogInformation("Email: {}, Password: {}", loginRequest.Value.Email, loginRequest.Value.Password);

            var response = CreateResponse(req, HttpStatusCode.OK);
            response.WriteString("Welcome to Azure Functions!!");

            return response;
        }

        private HttpResponseData CreateResponse(HttpRequestData req, HttpStatusCode statusCode)
        {
            var response = req.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            return response;
        }
    }
}
