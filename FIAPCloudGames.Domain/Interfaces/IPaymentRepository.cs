using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enumerators;

namespace FIAPCloudGames.Domain.Interfaces
{
    public interface IPaymentRepository : ICreate<Payment>, IFind<Payment>
    {
        Task<ICollection<Payment>?> FindAllPending();
        Task<ICollection<Payment>?> FindAllPendingByUserId(Guid userId);
        Task UpdateStatus(Guid id, PaymentStatus newStatus);
    }
}
