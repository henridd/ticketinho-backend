using Moq;
using NUnit.Framework;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;
using Ticketinho.Repository.Transactions;
using Ticketinho.Repository.Transactions.Operations;
using Ticketinho.Service.Negotiations;

namespace Ticketinho.UnitTests.Services.Negotiations
{
    public class NegotiationServiceTests
    {
        private Mock<INegotiationsRepository> _negotiationsRepositoryMock;
        private Mock<IUsersRepository> _usersRepositoryMock;
        private Mock<ITicketsRepository> _ticketsRepositoryMock;
        private Mock<IUnitOfWorkRunner> _unitOfWorkRunnerMock;

        [Test]
        public void AddAsync_TicketDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            var service = CreateService();
            var ticketId = "aa";
            var buyerId = "buyer";
            var sellerId = "seller";
            _ticketsRepositoryMock.Setup(x => x.GetByIdAsync(ticketId)).ReturnsAsync((Ticket)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddAsync(ticketId, buyerId, sellerId));
        }

        [Test]
        public void AddAsync_NegotiationForTicketExists_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var service = CreateService();

            var ticket = new Ticket() { Id = "aa" };
            _ticketsRepositoryMock.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var buyerId = "buyer";
            var sellerId = "seller";

            _negotiationsRepositoryMock.Setup(x => x.OpenNegotiationExistsAsync(ticket.Id)).ReturnsAsync(() => true);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.AddAsync(ticket.Id, buyerId, sellerId));
        }

        [Test]
        public void AddAsync_TicketIsNotActive_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var service = CreateService();

            var ticket = new Ticket() { Id = "aa", IsActive = false };
            _ticketsRepositoryMock.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var buyerId = "buyer";
            var sellerId = "seller";

            _negotiationsRepositoryMock.Setup(x => x.OpenNegotiationExistsAsync(ticket.Id)).ReturnsAsync(() => false);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.AddAsync(ticket.Id, buyerId, sellerId));
        }

        [Test]
        public void AddAsync_SellerDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            var service = CreateService();

            var ticket = new Ticket() { Id = "aa", IsActive = true };
            _ticketsRepositoryMock.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var buyerId = "buyer";

            var sellerId = "seller";
            _usersRepositoryMock.Setup(x => x.GetByIdAsync(sellerId)).ReturnsAsync((User)null);

            _negotiationsRepositoryMock.Setup(x => x.OpenNegotiationExistsAsync(ticket.Id)).ReturnsAsync(() => false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddAsync(ticket.Id, buyerId, sellerId));
        }

        [Test]
        public void AddAsync_BuyerDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            var service = CreateService();

            var ticket = new Ticket() { Id = "aa", IsActive = true };
            _ticketsRepositoryMock.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var buyerId = "buyer";
            _usersRepositoryMock.Setup(x => x.GetByIdAsync(buyerId)).ReturnsAsync((User)null);

            var seller = new User() { Id = "seller" };
            _usersRepositoryMock.Setup(x => x.GetByIdAsync(seller.Id)).ReturnsAsync(seller);

            _negotiationsRepositoryMock.Setup(x => x.OpenNegotiationExistsAsync(ticket.Id)).ReturnsAsync(() => false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddAsync(ticket.Id, buyerId, seller.Id));
        }

        [Test]
        public async Task AddAsync_ShouldSetTicketToInactive()
        {
            // Arrange
            var service = CreateService();

            var ticket = new Ticket() { Id = "aa", IsActive = true };
            _ticketsRepositoryMock.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var buyer = new User() { Id = "buyer" };
            _usersRepositoryMock.Setup(x => x.GetByIdAsync(buyer.Id)).ReturnsAsync(buyer);

            var seller = new User() { Id = "seller" };
            _usersRepositoryMock.Setup(x => x.GetByIdAsync(seller.Id)).ReturnsAsync(seller);

            _negotiationsRepositoryMock.Setup(x => x.OpenNegotiationExistsAsync(ticket.Id)).ReturnsAsync(() => false);

            _unitOfWorkRunnerMock.Setup(x => x.RunAsync(It.IsAny<UnitOfWork>())).ReturnsAsync(new List<string>() { "a" });

            // Act
            await service.AddAsync(ticket.Id, buyer.Id, seller.Id);

            // Assert
            _unitOfWorkRunnerMock.Verify(
                x => x.RunAsync(
                    It.Is<UnitOfWork>(
                        x => x.Operations.OfType<UpdateTransactionOperation>().Any(
                            y => y.PropertyName == nameof(ticket.IsActive) && ((bool)y.Value) == false))));
        }

        [Test]
        public async Task AddAsync_ShouldCreateNegotiation()
        {
            // Arrange
            var service = CreateService();

            var ticket = new Ticket() { Id = "aa", IsActive = true };
            _ticketsRepositoryMock.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var buyer = new User() { Id = "buyer" };
            _usersRepositoryMock.Setup(x => x.GetByIdAsync(buyer.Id)).ReturnsAsync(buyer);

            var seller = new User() { Id = "seller" };
            _usersRepositoryMock.Setup(x => x.GetByIdAsync(seller.Id)).ReturnsAsync(seller);

            _negotiationsRepositoryMock.Setup(x => x.OpenNegotiationExistsAsync(ticket.Id)).ReturnsAsync(() => false);

            _unitOfWorkRunnerMock.Setup(x => x.RunAsync(It.IsAny<UnitOfWork>())).ReturnsAsync(new List<string>() { "a" });

            // Act
            await service.AddAsync(ticket.Id, buyer.Id, seller.Id);

            // Assert
            _unitOfWorkRunnerMock.Verify(
                x => x.RunAsync(
                    It.Is<UnitOfWork>(
                        x => x.Operations.OfType<CreateTransactionOperation>().Any(
                            y => y.CollectionName == RepositoryNames.Negotiations))));
        }

        [Test]
        public void CancelNegotiationAsync_NegotiationDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            var service = CreateService();
            var id = "aa";
            _negotiationsRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Negotiation)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.CancelNegotiationAsync(id));
        }

        [Test]
        public void CancelNegotiationAsync_NegotiationIsNotActive_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var service = CreateService();
            var negotiation = new Negotiation() { Id = "aa", IsActive = false };
            _negotiationsRepositoryMock.Setup(x => x.GetByIdAsync(negotiation.Id)).ReturnsAsync(negotiation);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.CancelNegotiationAsync(negotiation.Id));
        }

        [Test]
        public async Task CancelNegotiationAsync_ShouldActivateTicket()
        {
            // Arrange
            var service = CreateService();
            var negotiation = new Negotiation() { Id = "aa", IsActive = true };
            _negotiationsRepositoryMock.Setup(x => x.GetByIdAsync(negotiation.Id)).ReturnsAsync(negotiation);

            // Act
            await service.CancelNegotiationAsync(negotiation.Id);

            // Assert
            _unitOfWorkRunnerMock.Verify(
                x => x.RunAsync(
                    It.Is<UnitOfWork>(
                        x => x.Operations.OfType<UpdateTransactionOperation>().Any(
                            y => y.PropertyName == "IsActive" && ((bool)y.Value) == true && y.CollectionName == RepositoryNames.Tickets))));
        }

        [Test]
        public async Task CancelNegotiationAsync_ShouldDeactivateNegotiation()
        {
            // Arrange
            var service = CreateService();
            var negotiation = new Negotiation() { Id = "aa", IsActive = true };
            _negotiationsRepositoryMock.Setup(x => x.GetByIdAsync(negotiation.Id)).ReturnsAsync(negotiation);

            // Act
            await service.CancelNegotiationAsync(negotiation.Id);

            // Assert
            _unitOfWorkRunnerMock.Verify(
                x => x.RunAsync(
                    It.Is<UnitOfWork>(
                        x => x.Operations.OfType<UpdateTransactionOperation>().Any(
                            y => y.PropertyName == nameof(negotiation.IsActive) && ((bool)y.Value) == false && y.CollectionName == RepositoryNames.Negotiations))));
        }

        [Test]
        public void PerformHandshakeAsync_NegotiationDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            var service = CreateService();
            var id = "aa";
            _negotiationsRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Negotiation)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.PerformHandshakeAsync(id));
        }

        [Test]
        public void PerformHandshakeAsync_NegotiationIsNotActive_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var service = CreateService();
            var negotiation = new Negotiation() { Id = "aa", IsActive = false };
            _negotiationsRepositoryMock.Setup(x => x.GetByIdAsync(negotiation.Id)).ReturnsAsync(negotiation);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.PerformHandshakeAsync(negotiation.Id));
        }

        [Test]
        public async Task PerformHandshake_ShouldPerformHandshake()
        {
            // Arrange
            var service = CreateService();
            var negotiation = new Negotiation() { Id = "aa", IsActive = true };
            _negotiationsRepositoryMock.Setup(x => x.GetByIdAsync(negotiation.Id)).ReturnsAsync(negotiation);

            // Act
            await service.PerformHandshakeAsync(negotiation.Id);

            // Assert
            _negotiationsRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.Is<Negotiation>(
                        z => z.HandshakePerformed && !z.IsActive)));
        }

        private NegotiationService CreateService()
        {
            _negotiationsRepositoryMock = new Mock<INegotiationsRepository>();
            _ticketsRepositoryMock = new Mock<ITicketsRepository>();
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _unitOfWorkRunnerMock = new Mock<IUnitOfWorkRunner>();

            return new NegotiationService(_negotiationsRepositoryMock.Object, _ticketsRepositoryMock.Object, _usersRepositoryMock.Object, _unitOfWorkRunnerMock.Object);
        }
    }
}
