using Ticketinho.Common.Enums;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;

namespace Ticketinho.Service.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IUsersRepository _usersRepository;

        public TicketService(ITicketsRepository ticketsRepository, IUsersRepository usersRepository)
        {
            _ticketsRepository = ticketsRepository ?? throw new ArgumentNullException(nameof(ticketsRepository));
            _usersRepository = usersRepository;
        }

        public async Task<string> AddAsync(string ownerId, TicketZone zone, TicketType type, double price)
        {
            var owner = _usersRepository.GetByIdAsync(ownerId);
            if(owner == null)
            {
                throw new ArgumentException($"There is no user with id {ownerId}", nameof(ownerId));
            }

            var ticket = new Ticket(ownerId, zone, type, price);

            return await _ticketsRepository.AddAsync(ticket);
        }

        public async Task<Ticket?> GetAsync(string id)
            => await _ticketsRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Ticket>> GetAllActiveAsync()
        {
            return await _ticketsRepository.GetAllActiveAsync();
        }

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
