using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Domain.Entities
{
    public class Payment : IEntity
    {
        public Payment() { }

        public Payment(Guid gameId, Guid userId) {
            Id = Guid.NewGuid();
            GameId = gameId;
            UserId = userId;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid GameId { get; private set; }
        public Guid UserId { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void UpdateStatus(PaymentStatus newStatus) => Status = newStatus;
    }
}
