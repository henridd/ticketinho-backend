using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ticketinho.Common.DTOs.Auth;
using Ticketinho.Functions;
using Ticketinho.Service.Auth;
using Ticketinho.Validation.Auth;

namespace Ticketinho
{
    public class AuthFunctions
    {
        private readonly ILogger _logger;
        private readonly IAuthService _authService;

        public AuthFunctions(ILoggerFactory loggerFactory, IAuthService authService)
        {
            _logger = loggerFactory.CreateLogger<AuthFunctions>();
            _authService = authService;
        }

        [Function("Users")]
        public async Task<HttpResponseData> RegisterUser([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var request = await req.GetJsonBody<CreateUserRequestDto, CreateUserRequestValidator>();

            if(!request.IsValid)
            {
                return await request.ToBadRequest(req);
            }

            await _authService.RegisterUserAsync(request.Value);
            

            return CreateResponse(req, HttpStatusCode.Created);

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

            var token = await _authService.LoginAsync(loginRequest.Value.Email);
            

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
