using EventCatalog.Domain.Models.EventAggregate;

namespace EventCatalog.Domain.Contracts
{
	public interface IEventRepository : IRepository<Event>
	{
		
	}
}