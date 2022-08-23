using System;
using System.Net.Http;
using System.Threading.Tasks;
using EventCatalog.Service.Models;

namespace EventCatalog.Service.ApplicationServices
{
	public class CreationService : ServiceBase
	{
		public async Task<Uri> CreateEventAsync(EventDto eventDto)
		{
			// HTTP POST request
			HttpResponseMessage response = await Client.PostAsJsonAsync(_query, eventDto);
			// throws an exception if the status code falls outside the range 200–299
			response.EnsureSuccessStatusCode();

			// return URI of the created resource.
			return response.Headers.Location;
		}
	}
}