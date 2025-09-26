using FIAPCloudGames.Domain.Entities;

namespace FIAPCloudGames.Domain.Interfaces
{
    public interface IPurchaseRepository : ICreate<Purchase>
    {
        Task<ICollection<Purchase>?> FindAllByUserId(Guid userId);
    }
}
