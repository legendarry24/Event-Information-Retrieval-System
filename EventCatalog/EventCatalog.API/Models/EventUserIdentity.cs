namespace EventCatalog.API.Models
{
	public class EventUserIdentity
	{
		public EventUserIdentity(Guid eventId, Guid userId)
		{
			EventId = eventId;
			UserId = userId;
		}

		public Guid EventId { get; }

		public Guid UserId { get; }
	}
}