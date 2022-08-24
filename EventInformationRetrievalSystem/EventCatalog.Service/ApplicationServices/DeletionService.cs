using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventCatalog.Service.ApplicationServices
{
	public class DeletionService : ServiceBase
	{
		public async Task<HttpStatusCode> DeleteEventAsync(Guid id)
		{
			// HTTP DELETE request
			HttpResponseMessage response = await Client.DeleteAsync($"{_query}{id}");

			return response.StatusCode;
		}
	}
}