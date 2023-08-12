using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;
using Ticketinho.Repository.Transactions;

namespace Ticketinho.Service.Negotiations
{

    public class NegotiationService : INegotiationService
    {
        private readonly INegotiationsRepository _negotiationsRepository;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IUnitOfWorkRunner _unitOfWorkRunner;

        public NegotiationService(INegotiationsRepository negotiationsRepository, ITicketsRepository ticketsRepository, IUsersRepository usersRepository, IUnitOfWorkRunner unitOfWorkRunner)
        {
            _negotiationsRepository = negotiationsRepository;
            _ticketsRepository = ticketsRepository;
            _usersRepository = usersRepository;
            _unitOfWorkRunner = unitOfWorkRunner;
        }

        public async Task<string> AddAsync(string ticketId, string buyerId, string sellerId)
        {
            var ticket = await _ticketsRepository.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new ArgumentException($"There is no ticket with id {ticketId}", nameof(ticketId));
            }

            var negotiationExists = await _negotiationsRepository.OpenNegotiationExistsAsync(ticketId);
            if (negotiationExists)
            {
                throw new InvalidOperationException($"There is already an open negotiation for the ticket {ticketId}");
            }

            if (!ticket.IsActive)
            {
                throw new InvalidOperationException($"Cannot create a negotiation for an inactive ticket");
            }

            var seller = await _usersRepository.GetByIdAsync(sellerId);
            if (seller == null)
            {
                throw new ArgumentException($"There is no user with id {sellerId}", nameof(sellerId));
            }

            var buyer = await _usersRepository.GetByIdAsync(buyerId);
            if (buyer == null)
            {
                throw new ArgumentException($"There is no user with id {buyerId}", nameof(buyerId));
            }

            var negotiation = new Negotiation(ticketId, sellerId, buyerId);

            var uow = new UnitOfWork()
                .WithUpdate(RepositoryNames.Tickets, ticket.Id, nameof(ticket.IsActive), false)
                .WithCreate(RepositoryNames.Negotiations, negotiation);

            var createdId = (await _unitOfWorkRunner.RunAsync(uow)).Single();

            return createdId;
        }

        public async Task CancelNegotiationAsync(string id)
        {
            var negotiation = await _negotiationsRepository.GetByIdAsync(id);
            if (negotiation == null)
            {
                throw new ArgumentException($"There is no negotiation with id {id}", nameof(id));
            }

            if (!negotiation.IsActive)
            {
                throw new InvalidOperationException($"The negotiation {id} is not active anymore");
            }

            var uow = new UnitOfWork()
                .WithUpdate(RepositoryNames.Tickets, negotiation.TicketId, "IsActive", true)
                .WithUpdate(RepositoryNames.Negotiations, negotiation.Id, nameof(negotiation.IsActive), false);

            await _unitOfWorkRunner.RunAsync(uow);
        }

        public async Task PerformHandshakeAsync(string id)
        {
            var negotiation = await _negotiationsRepository.GetByIdAsync(id);
            if (negotiation == null)
            {
                throw new ArgumentException($"There is no negotiation with id {id}", nameof(id));
            }

            if (!negotiation.IsActive)
            {
                throw new InvalidOperationException($"The negotiation {id} is not active anymore");
            }

            negotiation.PerformHandshake();

            await _negotiationsRepository.UpdateAsync(negotiation);
        }
    }
}
