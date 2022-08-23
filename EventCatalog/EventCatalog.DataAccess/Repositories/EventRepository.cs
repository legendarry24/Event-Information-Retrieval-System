using System;
using System.Collections.Generic;
using System.Linq;
using EventCatalog.Domain.Contracts;
using EventCatalog.Domain.Models.EventAggregate;
using Microsoft.EntityFrameworkCore;

namespace EventCatalog.DataAccess.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly DbSet<Event> _eventEntities;

		public EventRepository(EventCatalogContext context)
		{
			_eventEntities = context.Events;
		}

		public IEnumerable<Event> GetAll()
		{
			return _eventEntities
				.Include(e => e.PotentialAttendees)
				.ToList();
		}

		public Event GetById(Guid id)
		{
			var eventEntity = _eventEntities
				.Where(e => e.Id == id)
				.Include(e => e.PotentialAttendees)
				.FirstOrDefault();

			return eventEntity;
		}

		public void Add(Event entity)
		{
			_eventEntities.Add(entity);
		}

		public void Remove(Guid Id)
		{
			var eventEntity = _eventEntities.Find(Id);

			_eventEntities.Remove(eventEntity);
		}
	}
}