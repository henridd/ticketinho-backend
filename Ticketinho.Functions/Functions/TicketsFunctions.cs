using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Ticketinho.DTOs.Validation;
using Ticketinho.DTOs;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;
using Ticketinho.DTOs.Tickets;
using Ticketinho.DTOs.Validation.Tickets;

namespace Ticketinho.Functions
{
    internal class TicketsFunctions
    {
        private readonly ITicketsRepository ticketRepository;

        public TicketsFunctions(ITicketsRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        [Function("AddTicket")]
        public async Task<IActionResult> Add([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tickets")] HttpRequestData req)
        {
            var updateRequest = await req.GetJsonBody<UpdateTicketRequestDto, UpdateTicketRequestValidator>();
            await ticketRepository.AddAsync(new Ticket());

            return new OkResult();
        }

        [Function("DeleteTicket")]
        public async Task<IActionResult> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "tickets/{id}")] HttpRequestData req, string id)
        {
            await ticketRepository.DeleteAsync(id);

            return new OkResult();
        }

        [Function("GetTicket")]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tickets/{id}")] HttpRequestData req, string id)
        {
            var ticket = await ticketRepository.GetByIdAsync(id);

            return new OkObjectResult(ticket);
        }

        [Function("GetAllTickets")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tickets")] HttpRequestData req)
        {
            var tickets = await ticketRepository.GetAllAsync();

            return new OkObjectResult(tickets);
        }

        [Function("UpdateTicket")]
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tickets/{id}")] HttpRequestData req, string id)
        {
            var updateRequest = await req.GetJsonBody<UpdateTicketRequestDto, UpdateTicketRequestValidator>();
            var model = new Ticket()
            {
                Id = id,
                Price = updateRequest.Value.Price,
                Type = updateRequest.Value.Type,
                Zone = updateRequest.Value.Zone                
            };

            await ticketRepository.UpdateAsync(model);

            return new OkResult();
        }
    }
}
