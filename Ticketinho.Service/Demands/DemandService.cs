using Ticketinho.Common.Enums;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;

namespace Ticketinho.Service.Demands
{
    public class DemandService : IDemandService
    {
        private readonly IDemandsRepository _demandsRepository;
        private readonly IUsersRepository _usersRepository;

        public DemandService(IDemandsRepository demandsRepository, IUsersRepository usersRepository)
        {
            _demandsRepository = demandsRepository;
            _usersRepository = usersRepository;
        }

        public async Task<string> AddAsync(string requesterId, TicketZone zone, TicketType type, double price)
        {
            var requester = await _usersRepository.GetByIdAsync(requesterId);
            if (requester == null)
            {
                throw new ArgumentException($"There is no user with id {requesterId}", nameof(requesterId));
            }

            var demand = new Demand(requesterId, zone, type, price);

            return await _demandsRepository.AddAsync(demand);
        }

        public async Task DeleteAsync(string id) 
            => await _demandsRepository.DeleteAsync(id);

        public async Task<IEnumerable<Demand>> GetAllActiveAsync()
            => await _demandsRepository.GetAllActiveAsync();

        public async Task<Demand?> GetAsync(string id)
            => await _demandsRepository.GetByIdAsync(id);

        public async Task UpdateAsync(string id, TicketZone zone, TicketType type, double price)
        {
            var ticket = await _demandsRepository.GetByIdAsync(id) ?? throw new ArgumentException($"There is no demand with id {id}");

            ticket.Price = price;
            ticket.Type = type;
            ticket.Zone = zone;

            await _demandsRepository.UpdateAsync(ticket);
        }

        public async Task ReactivateDemandAsync(string id)
        {
            var ticket = await GetAsync(id) ?? throw new ArgumentException($"There is no demand with id {id}");

            await _demandsRepository.ReactivateAsync(ticket);
        }

        public async Task DeactivateOldDemandsAsync() 
            => await _demandsRepository.DeactivateDemandsAsync(DateTime.UtcNow);
    }
}
