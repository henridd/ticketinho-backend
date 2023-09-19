using FluentValidation;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using Ticketinho.Validation;

namespace Ticketinho
{
    public static class HttpRequestDataExtensions
    {
        public static async Task<HttpResponseData> CreateResponseWithContentAsync<T>(this HttpRequestData req, T content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = req.CreateResponse(statusCode);
            var jsonContent = JsonSerializer.Serialize(content, options: new JsonSerializerOptions(JsonSerializerDefaults.Web));
            response.Body = await new StringContent(jsonContent).ReadAsStreamAsync();
            return response;
        }

        public static async Task<HttpResponseData> CreateErrorResponseAsync<T>(this HttpRequestData req, T payload, HttpStatusCode statusCode)
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
            try
            {
                var requestBodyStr = await new StreamReader(request.Body).ReadToEndAsync();
                return JsonSerializer.Deserialize<T>(requestBodyStr);
            }
            catch(JsonException ex)
            {
                throw new HttpRequestException(ex.Message, null, HttpStatusCode.BadRequest);
            }
            
        }
    }
}