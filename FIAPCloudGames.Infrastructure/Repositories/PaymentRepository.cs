using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FIAPCloudGames.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly IEventRepository _event;
        public PaymentRepository(AppDbContext context, IEventRepository @event)
        {
            _context = context;
            _event = @event;
        }

        public async Task<Guid> Create(Payment model)
        {
            await _context.Payments.AddAsync(model);
            await _context.SaveChangesAsync();
            await _event.Save(new DomainEvent(model.Id, "PaymentCreated", JsonConvert.SerializeObject(model)));
            return model.Id;
        }

        public async Task<Payment?> Find(Guid id)
            => await _context.Payments.FindAsync(id);

        public async Task<ICollection<Payment>?> FindAllPending()
            => await _context.Payments.ToListAsync();

        public async Task<ICollection<Payment>?> FindAllPendingByUserId(Guid userId)
            => await _context.Payments.Where(x => x.UserId == userId).ToListAsync();

        public async Task UpdateStatus(Guid id, PaymentStatus newStatus)
        {
            var model = await Find(id);
            PaymentStatus oldStatus = model.Status;
            model.UpdateStatus(newStatus);
            _context.Payments.Update(model);
            await _context.SaveChangesAsync();
            await _event.Save(new DomainEvent(model.Id, "PaymentStatusUpdated", JsonConvert.SerializeObject(new { oldStatus = oldStatus.ToString(), newStatus = newStatus.ToString() })));
        }
    }
}
