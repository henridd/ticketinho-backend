
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Ticketinho
{
	public static class HttpRequestDataExtensions
	{
        public static async Task<HttpResponseData> CreateErrorResponseAsync(this HttpRequestData req, object payload, HttpStatusCode statusCode)
        {
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(payload, statusCode);
            return response;
        }
    }
}

