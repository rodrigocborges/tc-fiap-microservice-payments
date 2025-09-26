using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IPurchaseService _purchaseService;

        public PaymentService(IPaymentRepository repository, IPurchaseService purchaseService)
        {
            _repository = repository;
            _purchaseService = purchaseService;
        }

        public async Task<Guid> Create(Payment model)
            => await _repository.Create(model);

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

            //Para garantir que vai colocar a compra somente uma vez
            if (payment.Status != PaymentStatus.Approved && newStatus == PaymentStatus.Approved)
                await _purchaseService.Create(new Purchase(userId: payment.UserId, gameId: payment.UserId));
        }
    }
}
