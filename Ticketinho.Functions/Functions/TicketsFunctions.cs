using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Ticketinho.Common.DTOs.Tickets;
using Ticketinho.Service.Tickets;
using Ticketinho.Validation.Tickets;

namespace Ticketinho.Functions
{
    public class TicketsFunctions
    {
        private readonly ITicketService _ticketService;

        public TicketsFunctions(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [Function("AddTicket")]
        public async Task<HttpResponseData> Add([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tickets")] HttpRequestData req)
        {
            var createRequest = await req.GetJsonBody<CreateTicketRequestDto, CreateTicketRequestValidator>();
            if (!createRequest.IsValid)
            {
                return await createRequest.ToBadRequest(req);
            }

            var ticketId = await _ticketService.AddAsync(createRequest.Value.OwnerId,
                                                         createRequest.Value.Zone,
                                                         createRequest.Value.Type,
                                                         createRequest.Value.Price);

            return await req.CreateResponseWithContentAsync(ticketId);
        }

        [Function("DeleteTicket")]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "tickets/{id}")] HttpRequestData req, string id)
        {
            await _ticketService.DeleteAsync(id);

            return req.CreateResponse();
        }

        [Function("GetTicket")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tickets/{id}")] HttpRequestData req, string id)
        {
            var ticket = await _ticketService.GetAsync(id);

            if (ticket == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            return await req.CreateResponseWithContentAsync(ticket);
        }

        [Function("GetAllTickets")]
        public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tickets")] HttpRequestData req)
        {
            var tickets = await _ticketService.GetAllActiveAsync();

            return await req.CreateResponseWithContentAsync(tickets);
        }

        [Function("UpdateTicket")]
        public async Task<HttpResponseData> Update([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tickets/{id}")] HttpRequestData req, string id)
        {
            var updateRequest = await req.GetJsonBody<UpdateTicketRequestDto, UpdateTicketRequestValidator>();

            await _ticketService.UpdateAsync(id,
                                             updateRequest.Value.Zone,
                                             updateRequest.Value.Type,
                                             updateRequest.Value.Price);

            return req.CreateResponse();
        }

        [Function("ReactivateTicket")]
        public async Task<HttpResponseData> ReactivateTicket([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "tickets/{id}/reactivate")] HttpRequestData req, string id)
        {
            await _ticketService.ReactivateTicketAsync(id);

            return req.CreateResponse();
        }

        [Function("Deactivate old tickets")]
        public async Task Run([TimerTrigger("0 0 4 * * *")] TimerInfo timer)
        {
            await _ticketService.DeactivateOldTicketsAsync();
        }
    }
}
