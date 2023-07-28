using Microsoft.Azure.Functions.Worker.Http;

namespace Ticketinho.DTOs.Validation
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
			var response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
			var validationErrors = request.Errors.Select(e => new ValidationError{
				Field = e.PropertyName,
				Error = e.ErrorMessage
			});

			await response.WriteAsJsonAsync(validationErrors);
			return response;
		}
	}
}

