using FIAPCloudGames.Domain.Entities;

namespace FIAPCloudGames.Domain.Interfaces
{
    public interface IPurchaseService : ICreate<Purchase>
    {
        Task<ICollection<Purchase>?> FindAllByUserId(Guid userId);
    }
}
