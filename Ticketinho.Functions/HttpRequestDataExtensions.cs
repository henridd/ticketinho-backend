using FluentValidation;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using Ticketinho.Common.DTOs.Validation;

namespace Ticketinho
{
    public static class HttpRequestDataExtensions
    {
        public static async Task<HttpResponseData> CreateResponseWithContentAsync(this HttpRequestData req, object content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = req.CreateResponse(statusCode);
            var jsonContent = JsonSerializer.Serialize(content);
            response.Body = await new StringContent(jsonContent).ReadAsStreamAsync();
            return response;
        }

        public static async Task<HttpResponseData> CreateErrorResponseAsync(this HttpRequestData req, object payload, HttpStatusCode statusCode)
        {
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(payload, statusCode);
            return response;
        }

        public static async Task<ValidableRequest<T>> GetJsonBody<T, V>(this HttpRequestData request)
            where V : AbstractValidator<T>, new()
        {
            var requestObject = await request.GetJsonBody<T>() ?? throw new HttpRequestException("Invalid request body", null, HttpStatusCode.BadRequest);

            var validator = new V();
            var validationResult = validator.Validate(requestObject);

            if (!validationResult.IsValid)
            {
                return new ValidableRequest<T>
                {
                    Value = requestObject,
                    IsValid = false,
                    Errors = validationResult.Errors
                };
            }

            return new ValidableRequest<T>
            {
                Value = requestObject,
                IsValid = true
            };
        }

        public static async Task<T?> GetJsonBody<T>(this HttpRequestData request)
        {
            var requestBodyStr = await new StreamReader(request.Body).ReadToEndAsync();
            return JsonSerializer.Deserialize<T>(requestBodyStr);
        }
    }
}