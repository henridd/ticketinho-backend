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

            if (!request.IsValid)
            {
                return await request.ToBadRequest(req);
            }

            await _authService.RegisterUserAsync(request.Value);
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;
        }

        [Function("Login")]
        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var loginRequest = await req.GetJsonBody<LoginRequestDto, LoginRequestValidator>();

            if (!loginRequest.IsValid)
            {
                return await loginRequest.ToBadRequest(req);
            }

            var payload = await _authService.LoginAsync(loginRequest.Value);
            if (payload.Email is null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            if (payload.Token is null)
            {
                return req.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var response = await CreateResponseAsync(req, payload, HttpStatusCode.OK);
            return response;
        }

        private async Task<HttpResponseData> CreateResponseAsync<T>(HttpRequestData req, T payload, HttpStatusCode statusCode)
        {
            var response = req.CreateResponse(statusCode);
            await response.WriteAsJsonAsync(payload);
            return response;
        }
    }
}
