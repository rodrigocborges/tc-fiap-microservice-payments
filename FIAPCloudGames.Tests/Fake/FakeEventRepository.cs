using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Tests.Fake
{
    public class FakeEventRepository : IEventRepository
    {
        public List<DomainEvent> Events = new List<DomainEvent>();

        public Task Save(DomainEvent domainEvent)
        {
            Events.Add(domainEvent);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<DomainEvent>> GetAllByAggregateId(Guid aggregateId)
        {
            var results = Events.Where(e => e.AggregateId == aggregateId);
            return Task.FromResult(results.AsEnumerable());
        }

        public Task<IEnumerable<DomainEvent>> GetAll(int skip, int take)
        {
            return Task.FromResult(Events.Skip(skip).Take(take).AsEnumerable());
        }
    }
}