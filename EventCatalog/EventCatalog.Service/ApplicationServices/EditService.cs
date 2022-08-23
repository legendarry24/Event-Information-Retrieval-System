using System.Net.Http;
using System.Threading.Tasks;
using EventCatalog.Service.Models;

namespace EventCatalog.Service.ApplicationServices
{
	public class EditService : ServiceBase
	{
		public async Task<EventDto> UpdateEventAsync(EventDto eventDto)
		{
			// HTTP PUT request
			HttpResponseMessage response = await Client.PutAsJsonAsync($"{_query}{eventDto.Id}", eventDto);
			response.EnsureSuccessStatusCode();

			// Deserialize the updated event from the response body.
			eventDto = await response.Content.ReadAsAsync<EventDto>();

			return eventDto;
		}

		public async Task AddEventToFavoritesAsync(EventUserIdentity eventUserIdentity)
		{
			// HTTP PUT request
			HttpResponseMessage response = await Client.PutAsJsonAsync(
				$"{_query}addToFavorites/{eventUserIdentity.EventId}",
				eventUserIdentity);

			response.EnsureSuccessStatusCode();
		}
	}
}