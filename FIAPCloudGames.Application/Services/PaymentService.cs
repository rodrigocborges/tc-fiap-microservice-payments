using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Events;
using FIAPCloudGames.Domain.Interfaces;
using MassTransit;

namespace FIAPCloudGames.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IPurchaseService _purchaseService;
        private readonly IPublishEndpoint _publishEndpoint;

        public PaymentService(IPaymentRepository repository, IPurchaseService purchaseService, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _purchaseService = purchaseService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Create(Payment model)
        {
            Guid id = await _repository.Create(model);
            if(_publishEndpoint != null)
                await _publishEndpoint.Publish(new PaymentCreatedEvent(model.Id, model.GameId, model.UserId, 100.00m));
            return id;
        }

        public async Task<Payment?> Find(Guid id)
            => await _repository.Find(id);

        public async Task<ICollection<Payment>?> FindAllPending()
            => await _repository.FindAllPending();

        public async Task<ICollection<Payment>?> FindAllPendingByUserId(Guid userId)
            => await _repository.FindAllPendingByUserId(userId);

        public async Task UpdateStatus(Guid id, PaymentStatus newStatus)
        {
            var payment = await Find(id);
            if(payment == null)
                return;
            
            await _repository.UpdateStatus(id, newStatus);

            if (newStatus == PaymentStatus.Approved)
                await _purchaseService.Create(new Purchase(userId: payment.UserId, gameId: payment.UserId));
        }
    }
}
