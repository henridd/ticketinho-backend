using Moq;
using NUnit.Framework;
using Ticketinho.Common.Enums;
using Ticketinho.Repository.Models;
using Ticketinho.Repository.Repositories;
using Ticketinho.Service.Demands;

namespace Ticketinho.UnitTests.Services.Demands
{
    public class DemandServiceTests
    {
        private Mock<IDemandsRepository> _demandsRepositoryMock;
        private Mock<IUsersRepository> _usersRepositoryMock;

        [Test]
        public void AddAsync_InexistentRequester_ShouldThrow()
        {
            // Arrange
            var service = CreateService();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddAsync("me", TicketZone.A, TicketType.Adult, 1));
        }

        [Test]
        public async Task AddAsync_ShouldAddToRepository()
        {
            // Arrange
            var service = CreateService();
            var id = "you";
            var zone = TicketZone.A | TicketZone.B;
            var type = TicketType.Adult;
            var price = 33.5;

            _usersRepositoryMock!.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new User());

            // Act
            await service.AddAsync(id, zone, type, price);

            // Assert
            _demandsRepositoryMock!.Verify(x => x.AddAsync(It.Is<Demand>(
                t => t.RequesterId == id &&
                t.Type == type &&
                t.Price == price &&
                t.Zone == zone)));
        }

        [Test]
        public async Task DeleteAsync_ShouldDelete()
        {
            // Arrange
            const string Id = "demand";
            var service = CreateService();

            // Act
            await service.DeleteAsync(Id);

            // Assert
            _demandsRepositoryMock.Verify(x => x.DeleteAsync(Id));
        }

        [Test]
        public void UpdateAsync_InexistentDemand_ShouldThrow()
        {
            // Arrange
            var service = CreateService();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateAsync("no1", TicketZone.A, TicketType.Adult, 1));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateAllFields()
        {
            // Arrange
            var service = CreateService();
            var id = "please sell";
            var zone = TicketZone.A | TicketZone.C;
            var type = TicketType.Adult;
            var price = 2321.1;

            var demand = new Demand();

            _demandsRepositoryMock!.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(demand);

            // Act
            await service.UpdateAsync(id, zone, type, price);

            // Assert
            _demandsRepositoryMock!.Verify(x => x.UpdateAsync(It.Is<Demand>(
                t => t.Type == type &&
                t.Price == price &&
                t.Zone == zone)));
        }

        [Test]
        public void ReactivateTicketAsync_InexistentTicket_ShouldThrow()
        {
            // Arrange
            var service = CreateService();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.ReactivateDemandAsync("..."));
        }

        private DemandService CreateService()
        {
            _demandsRepositoryMock = new Mock<IDemandsRepository>();
            _usersRepositoryMock = new Mock<IUsersRepository>();

            return new DemandService(_demandsRepositoryMock.Object, _usersRepositoryMock.Object);
        }
    }
}
