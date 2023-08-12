using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Ticketinho.Common.DTOs.Negotiation;
using Ticketinho.Service.Negotiations;
using Ticketinho.Validation.Negotiation;

namespace Ticketinho.Functions
{
    public class NegotiationFunctions
    {
        private readonly INegotiationService _negotiationService;

        public NegotiationFunctions(INegotiationService negotiationService)
        {
            _negotiationService = negotiationService;
        }

        [Function("CreateNegotiation")]
        public async Task<HttpResponseData> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "negotiations")] HttpRequestData req)
        {
            var createRequest = await req.GetJsonBody<CreateNegotiationRequestDto, CreateNegotiationRequestValidator>();
            if (!createRequest.IsValid)
            {
                return await createRequest.ToBadRequest(req);
            }

            var ticketId = await _negotiationService.AddAsync(createRequest.Value.TicketId,
                                                              createRequest.Value.BuyerId,
                                                              createRequest.Value.SellerId);

            return await req.CreateResponseWithContentAsync(ticketId);
        }

        [Function("PerformHandshake")]
        public async Task<HttpResponseData> PerformHandshake([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "negotiations/{id}/accept")] HttpRequestData req, string id)
        {
            await _negotiationService.PerformHandshakeAsync(id);

            return req.CreateResponse();
        }

        [Function("CancelNegotiation")]
        public async Task<HttpResponseData> CancelNegotiation([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "negotiations/{id}/cancel")] HttpRequestData req, string id)
        {
            await _negotiationService.CancelNegotiationAsync(id);

            return req.CreateResponse();
        }
    }
}
