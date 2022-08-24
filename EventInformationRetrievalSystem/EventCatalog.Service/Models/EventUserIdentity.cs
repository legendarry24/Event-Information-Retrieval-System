using System;

namespace EventCatalog.Service.Models
{
	public class EventUserIdentity
	{
		public EventUserIdentity(Guid eventId, Guid userId)
		{
			EventId = eventId;
			UserId = userId;
		}

		public Guid EventId { get; private set; }

		public Guid UserId { get; private set; }
	}
}