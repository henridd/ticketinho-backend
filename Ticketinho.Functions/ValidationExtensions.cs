using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace Ticketinho.Functions
{
	public class ValidationError
	{
		public required string Field { get; set; }
        public required string Error { get; set; }
    }

	public static class ValidationExtensions
	{
		public static async Task<HttpResponseData> ToBadRequest<T>(this ValidableRequest<T> request, HttpRequestData req)
		{
			
			var validationErrors = request.Errors.Select(e => new ValidationError{
				Field = e.PropertyName,
				Error = e.ErrorMessage
			});

			var response = await req.CreateErrorResponseAsync(validationErrors, HttpStatusCode.BadRequest);
			return response;
		}
	}
}

