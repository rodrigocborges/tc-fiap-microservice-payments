using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Tests.Fake;

namespace FIAPCloudGames.Tests
{
    public class PurchaseServiceTest
    {
        private readonly FakeEventRepository _fakeEventRepository;

        public PurchaseServiceTest()
        {
            _fakeEventRepository = new FakeEventRepository();
        }

        [Fact]
        public async Task CreatePurchase_WithValidData_ShouldCreateAndReturnId()
        {
            // Arrange
            var purchaseRepository = new FakePurchaseRepository(_fakeEventRepository);
            var service = new PurchaseService(purchaseRepository);

            var gameId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var purchase = new Purchase(userId, gameId);

            // Act
            var resultId = await service.Create(purchase);

            // Assert
            Assert.Equal(purchase.Id, resultId);

            // Para verificar se foi criado, tentamos encontrá-lo na lista do fake repo
            var userPurchases = await purchaseRepository.FindAllByUserId(userId);
            var createdPurchase = userPurchases?.FirstOrDefault(p => p.Id == resultId);

            Assert.NotNull(createdPurchase);
            Assert.Equal(userId, createdPurchase.UserId);
            Assert.Equal(gameId, createdPurchase.GameId);

            // Verifica se o evento foi guardado
            Assert.Single(_fakeEventRepository.Events);
            Assert.Equal("PurchaseCreated", _fakeEventRepository.Events.First().EventType);
        }
    }
}