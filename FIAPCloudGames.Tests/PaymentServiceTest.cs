using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Tests.Fake;

namespace FIAPCloudGames.Tests
{
    public class PaymentServiceTest
    {
        private readonly FakeEventRepository _fakeEventRepository;

        public PaymentServiceTest()
        {
            _fakeEventRepository = new FakeEventRepository();
        }

        [Fact]
        public async Task CreatePayment_WithValidData_ShouldCreateAndReturnId()
        {
            // Arrange
            var paymentRepository = new FakePaymentRepository(_fakeEventRepository);
            var service = new PaymentService(paymentRepository, null, null);

            var gameId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var payment = new Payment(gameId, userId);

            // Act
            var resultId = await service.Create(payment);

            // Assert
            Assert.Equal(payment.Id, resultId);

            var createdPayment = await paymentRepository.Find(resultId);
            Assert.NotNull(createdPayment);
            Assert.Equal(userId, createdPayment.UserId);
            Assert.Equal(gameId, createdPayment.GameId);

            // Verifica se o evento de criação foi guardado
            Assert.Single(_fakeEventRepository.Events);
            Assert.Equal("PaymentCreated", _fakeEventRepository.Events.First().EventType);
        }
    }
}