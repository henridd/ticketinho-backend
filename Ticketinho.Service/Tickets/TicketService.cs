using Ticketinho.Repository.Models;
using Ticketinho.Repository.Models.Enums;
using Ticketinho.Repository.Repositories;

namespace Ticketinho.Service.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketsRepository _ticketsRepository;

        public TicketService(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository ?? throw new ArgumentNullException(nameof(ticketsRepository));
        }

        public async Task<string> AddAsync(string ownerId, TicketZone zone, TicketType type, double price)
        {
            // TODO: Pesquisar e validar se o owner é válido

            var ticket = new Ticket(ownerId, zone, type, price);

            return await _ticketsRepository.AddAsync(ticket);
        }

        public async Task<Ticket?> GetAsync(string id)
            => await _ticketsRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Ticket>> GetAllAsync()
            => await _ticketsRepository.GetAllAsync();

        public async Task UpdateAsync(string id, TicketZone zone, TicketType type, double price)
        {
            var ticket = await _ticketsRepository.GetByIdAsync(id) ?? throw new ArgumentException($"There is no ticket with id {id}");

            ticket.Price = price;
            ticket.Type = type;
            ticket.Zone = zone;

            await _ticketsRepository.UpdateAsync(ticket);
        }

        public async Task DeleteAsync(string ticketId)
            => await _ticketsRepository.DeleteAsync(ticketId);

        public async Task ReactivateTicketAsync(string id)
        {
            var ticket = await GetAsync(id) ?? throw new ArgumentException($"There is no ticket with id {id}");

            await _ticketsRepository.ReactivateAsync(ticket);
        }

        public async Task DeactivateOldTicketsAsync()
        {
            await _ticketsRepository.DeactivateTicketsAsync(DateTime.UtcNow);
        }
    }
}
