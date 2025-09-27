using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Tests.Fake
{
    public class FakePaymentRepository : IPaymentRepository
    {
        private readonly List<Payment> _payments = new List<Payment>();
        private readonly IEventRepository _eventRepository;

        public FakePaymentRepository(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Guid> Create(Payment model)
        {
            _payments.Add(model);
            // Simulamos também o armazenamento do evento
            await _eventRepository.Save(new DomainEvent(model.Id, "PaymentCreated", "{}"));
            return model.Id;
        }

        public Task<Payment?> Find(Guid id)
        {
            var payment = _payments.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(payment);
        }

        public Task<ICollection<Payment>?> FindAllPending()
        {
            var results = _payments.Where(p => p.Status == PaymentStatus.Pending).ToList();
            return Task.FromResult<ICollection<Payment>?>(results);
        }

        public Task<ICollection<Payment>?> FindAllPendingByUserId(Guid userId)
        {
            var results = _payments.Where(p => p.UserId == userId && p.Status == PaymentStatus.Pending).ToList();
            return Task.FromResult<ICollection<Payment>?>(results);
        }

        public async Task UpdateStatus(Guid id, PaymentStatus newStatus)
        {
            var model = await Find(id);
            if (model != null)
            {
                model.UpdateStatus(newStatus);
                await _eventRepository.Save(new DomainEvent(model.Id, "PaymentStatusUpdated", "{}"));
            }
        }
    }
}