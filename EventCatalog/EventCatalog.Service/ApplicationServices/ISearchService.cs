using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventCatalog.Service.Models;

namespace EventCatalog.Service.ApplicationServices
{
	public interface ISearchService
	{
		Task<IEnumerable<EventDto>> GetEventsAsync();

		Task<EventDto> GetEventByIdAsync(Guid id);

		IEnumerable<EventDto> FindByFilter(Expression<Func<EventDto, bool>> filter);
	}
}