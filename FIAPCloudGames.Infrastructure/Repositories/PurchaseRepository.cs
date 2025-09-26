using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FIAPCloudGames.Infrastructure.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _context;
        private readonly IEventRepository _event;
        public PurchaseRepository(AppDbContext context, IEventRepository @event)
        {
            _context = context;
            _event = @event;
        }

        public async Task<Guid> Create(Purchase model)
        {
            await _context.Purchases.AddAsync(model);
            await _context.SaveChangesAsync();
            await _event.Save(new DomainEvent(model.Id, "PurchaseCreated", JsonConvert.SerializeObject(model)));
            return model.Id;
        }

        public async Task<ICollection<Purchase>?> FindAllByUserId(Guid userId)
            => await _context.Purchases.Where(x => x.UserId == userId).ToListAsync();
    }
}
