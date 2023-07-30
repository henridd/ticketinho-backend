using Moq;
using NUnit.Framework;
using Ticketinho.Common.Enums;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;
using Ticketinho.Service.Tickets;

namespace Ticketinho.UnitTests.Services.Tickets
{
    public class TicketServiceTests
    {
        private Mock<ITicketsRepository>? _ticketsRepositoryMock;
        private Mock<IUsersRepository>? _usersRepositoryMock;

        [Test]
        public void AddAsync_UnexistingOwner_ShouldThrow()
        {
            // Arrange
            var service = CreateService();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddAsync("pikachu", TicketZone.A, TicketType.Adult, 1));
        }

        [Test]
        public async Task AddAsync_ShouldAddToRepository()
        {
            // Arrange
            var service = CreateService();
            var id = "charmander";
            var zone = TicketZone.A | TicketZone.B;
            var type = TicketType.Adult;
            var price = 33.5;

            _usersRepositoryMock!.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new User());

            // Act
            await service.AddAsync(id, zone, type, price);

            // Assert
            _ticketsRepositoryMock!.Verify(x => x.AddAsync(It.Is<Ticket>(
                t => t.OwnerId == id &&
                t.Type == type &&
                t.Price == price &&
                t.Zone == zone)));
        }

        [Test]
        public void UpdateAsync_UnexistingTicket_ShouldThrow()
        {
            // Arrange
            var service = CreateService();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateAsync("kanto", TicketZone.A, TicketType.Adult, 1));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateAllFields()
        {
            // Arrange
            var service = CreateService();
            var id = "pikachu";
            var zone = TicketZone.A | TicketZone.C;
            var type = TicketType.Adult;
            var price = 2321.1;

            var ticket = new Ticket();

            _ticketsRepositoryMock!.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(ticket);

            // Act
            await service.UpdateAsync(id, zone, type, price);

            // Assert
            _ticketsRepositoryMock!.Verify(x => x.UpdateAsync(It.Is<Ticket>(
                t => t.Type == type &&
                t.Price == price &&
                t.Zone == zone)));
        }

        [Test]
        public void ReactivateTicketAsync_UnexistingTicket_ShouldThrow()
        {
            // Arrange
            var service = CreateService();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.ReactivateTicketAsync("johto"));
        }

        private TicketService CreateService()
        {
            _ticketsRepositoryMock = new Mock<ITicketsRepository>();
            _usersRepositoryMock = new Mock<IUsersRepository>();

            return new TicketService(_ticketsRepositoryMock.Object, _usersRepositoryMock.Object);
        }
    }
}
