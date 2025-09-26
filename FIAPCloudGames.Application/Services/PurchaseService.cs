using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Application.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;

        public PurchaseService(IPurchaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Create(Purchase model)
            => await _repository.Create(model);

        public async Task<ICollection<Purchase>?> FindAllByUserId(Guid userId)
            => await _repository.FindAllByUserId(userId);
    }
}
