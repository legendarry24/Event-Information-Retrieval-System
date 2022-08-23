using System;
using EventCatalog.Domain.Models.EventAggregate;

namespace EventCatalog.Domain.Models
{
	public class FavoriteEvent
	{
		public Guid EventId { get; set; }

		public Guid UserId { get; set; }

		public Event Event { get; set; }

		public User User { get; set; }
	}
}