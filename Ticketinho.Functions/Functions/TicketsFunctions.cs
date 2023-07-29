using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;

namespace Ticketinho.Functions
{
    internal class TicketsFunctions
    {
        private readonly ITicketsRepository ticketRepository;

        public TicketsFunctions(ITicketsRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        [Function("Test")]
        public async Task<IActionResult> Test([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            await ticketRepository.AddAsync(new Ticket());

            return new OkResult();
        }
    }
}
