using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Tests.Fake
{
    public class FakePurchaseRepository : IPurchaseRepository
    {
        private readonly List<Purchase> _purchases = new List<Purchase>();
        private readonly IEventRepository _eventRepository;

        public FakePurchaseRepository(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Guid> Create(Purchase model)
        {
            _purchases.Add(model);
            await _eventRepository.Save(new DomainEvent(model.Id, "PurchaseCreated", "{}"));
            return model.Id;
        }

        public Task<ICollection<Purchase>?> FindAllByUserId(Guid userId)
        {
            var results = _purchases.Where(p => p.UserId == userId).ToList();
            return Task.FromResult<ICollection<Purchase>?>(results);
        }
    }
}