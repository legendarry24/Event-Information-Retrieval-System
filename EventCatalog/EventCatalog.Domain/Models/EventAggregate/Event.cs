using System;
using System.Collections.Generic;
using EventCatalog.Domain.Contracts;

namespace EventCatalog.Domain.Models.EventAggregate
{
	public class Event : IEntity
	{
		public Event(string name, EventType type, Address location, EventDetails eventDetails)
		{
			Id = Guid.NewGuid();
			Name = name;
			Type = type;
			Location = location;
			EventDetails = eventDetails;
		}

		public Event() { }

		public Guid Id { get; private set; }

		public string Name { get; set; }

		public EventType Type { get; set; }

		public Address Location { get; set; }

		public EventDetails EventDetails { get; set; }

		public ICollection<FavoriteEvent> PotentialAttendees { get; set; }
	}
}