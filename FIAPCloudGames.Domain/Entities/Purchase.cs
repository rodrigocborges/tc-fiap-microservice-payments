using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Domain.Entities
{
    public class Purchase : IEntity
    {
        public Purchase() { }

        public Purchase(Guid userId, Guid gameId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            GameId = gameId;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid GameId { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}
