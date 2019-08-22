using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EventCatalog.Service.ApplicationServices
{
	public abstract class ServiceBase
	{
		private const string _baseUri = "https://localhost:44334/";

		protected HttpClient Client { get; }

		protected const string _query = "api/events/";

		protected static string AbsolutePath => _baseUri + _query;

		protected ServiceBase()
		{
			Client = new HttpClient();
			Client.BaseAddress = new Uri(_baseUri);
			Client.DefaultRequestHeaders.Accept.Clear();
			// set Accept header to "application/json"
			Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}