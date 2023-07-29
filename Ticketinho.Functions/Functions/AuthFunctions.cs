using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ticketinho.DTOs;
using Ticketinho.DTOs.Validation;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;

namespace Ticketinho
{
    public class AuthFunctions
    {
        private readonly ILogger _logger;
        private readonly IUsersRepository _usersRepository;

        public AuthFunctions(ILoggerFactory loggerFactory, IUsersRepository usersRepository)
        {
            _logger = loggerFactory.CreateLogger<AuthFunctions>();
            _usersRepository = usersRepository;
        }

        [Function("Users")]
        public async Task<HttpResponseData> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var request = await req.GetJsonBody<CreateUserRequestDto, CreateUserRequestValidator>();

            if(!request.IsValid)
            {
                return await request.ToBadRequest(req);
            }

            await _usersRepository.AddAsync(
                new User(
                    request.Value.Name,
                    request.Value.Email,
                    request.Value.Password,
                    request.Value.PhoneNumber
                    )
                );

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
