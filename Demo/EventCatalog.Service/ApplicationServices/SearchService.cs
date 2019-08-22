using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using EventCatalog.Service.Models;

namespace EventCatalog.Service.ApplicationServices
{
	public class SearchService : ServiceBase, ISearchService
	{
		public async Task<IEnumerable<EventDto>> GetEventsAsync()
		{
			IEnumerable<EventDto> events = null;
			// HTTP GET request
			HttpResponseMessage response = await Client.GetAsync(AbsolutePath);

			if (response.IsSuccessStatusCode)
			{
				// deserialize JSON to EventDto
				events = await response.Content.ReadAsAsync<IEnumerable<EventDto>>();
			}

			return events;
		}

		public async Task<EventDto> GetEventByIdAsync(Guid id)
		{
			EventDto eventDto = null;
			// HTTP GET request
			HttpResponseMessage response = await Client.GetAsync(AbsolutePath + id);

			if (response.IsSuccessStatusCode)
			{
				// deserialize JSON to EventDto
				eventDto = await response.Content.ReadAsAsync<EventDto>();
			}

			return eventDto;
		}

		public IEnumerable<EventDto> FindByFilter(Expression<Func<EventDto, bool>> filter)
		{
			throw new NotImplementedException();
		}
	}
}