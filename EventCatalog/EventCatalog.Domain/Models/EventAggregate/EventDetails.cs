using System;

namespace EventCatalog.Domain.Models.EventAggregate
{
	public class EventDetails
	{
		public EventDetails(
			DateTime startTime,
			DateTime endTime,
			string organizerSite,
			Money price,
			string description,
			string image)
		{
			StartTime = startTime;
			EndTime = endTime;
			OrganizerSite = organizerSite;
			Price = price;
			Description = description;
			Image = image;
		}

		public EventDetails() { }

		public DateTime StartTime { get; private set; }

		public DateTime EndTime { get; private set; }

		public string OrganizerSite { get; private set; }

		public Money Price { get; private set; }

		public string Description { get; private set; }

		public string Image { get; private set; }
	}
}