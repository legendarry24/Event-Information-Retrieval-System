using EventCatalog.API.Models;
using EventCatalog.Domain.Contracts;
using EventCatalog.Domain.Models;
using EventCatalog.Domain.Models.EventAggregate;
using Microsoft.AspNetCore.Mvc;

namespace EventCatalog.API.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class EventsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public EventsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET api/events
		[HttpGet]
		public IActionResult GetEvents()
		{
			var eventEntities = _unitOfWork.EventRepository.GetAll();

			var eventsToReturn = eventEntities.Select(MapModel).ToList();

			return Ok(eventsToReturn);
		}

		// GET api/events/5
		[HttpGet("{id}")]
		public IActionResult GetEvent(Guid id)
		{
			Event? eventEntity = _unitOfWork.EventRepository.GetById(id);

			if (eventEntity == null)
			{
				return NotFound();
			}

			EventDto eventToReturn = MapModel(eventEntity);

			return Ok(eventToReturn);
		}

		// POST api/events
		[HttpPost]
		public IActionResult CreateEvent([FromBody] EventDto? eventDto)
		{
			if (eventDto == null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Event eventEntity = MapModel(eventDto);

			_unitOfWork.EventRepository.Add(eventEntity);
			_unitOfWork.Save();

			return CreatedAtAction(nameof(GetEvent), new { id = eventEntity.Id }, eventEntity);
		}

		// PUT api/events/5
		[HttpPut("{id}")]
		public IActionResult UpdateEvent(Guid id, [FromBody] EventDto? eventDto)
		{
			if (eventDto == null)
			{
				return BadRequest();
			}

			Event? eventEntity = _unitOfWork.EventRepository.GetById(id);
			if (eventEntity == null)
			{
				return NotFound();
			}

			eventEntity.Name = eventDto.Name;
			eventEntity.Type = eventDto.Type;
			eventEntity.Location = new Address(
				eventDto.City,
				eventDto.Street,
				eventDto.Venue);
			eventEntity.EventDetails = new EventDetails(
				eventDto.StartTime,
				eventDto.EndTime,
				eventDto.OrganizerSite,
				new Money(eventDto.Price, eventDto.Currency),
				eventDto.Description,
				eventDto.ImageFileName);

			_unitOfWork.Save();

			return NoContent();
		}

		// PUT api/events/addToFavorites/5
		[HttpPut("addToFavorites/{eventId}")]
		public IActionResult AddToFavorites(Guid eventId, [FromBody] EventUserIdentity eventUserIdentity)
		{
			Event? eventEntity = _unitOfWork.EventRepository.GetById(eventId);
			if (eventEntity == null)
			{
				return NotFound();
			}

			if (eventEntity.Id != eventUserIdentity.EventId)
			{
				return BadRequest();
			}

			bool entryExists = eventEntity.PotentialAttendees.Any(f =>
				f.UserId == eventUserIdentity.UserId && f.EventId == eventEntity.Id);

			if (!entryExists)
			{
				eventEntity.PotentialAttendees.Add(new FavoriteEvent
				{
					EventId = eventEntity.Id,
					UserId = eventUserIdentity.UserId
				});

				_unitOfWork.Save();
			}

			return NoContent();
		}

		// DELETE api/events/5
		[HttpDelete("{id}")]
		public IActionResult DeleteEvent(Guid id)
		{
			Event? eventEntity = _unitOfWork.EventRepository.GetById(id);
			if (eventEntity == null)
			{
				return NotFound();
			}

			_unitOfWork.EventRepository.Remove(id);
			_unitOfWork.Save();

			return NoContent();
		}

		private static Event MapModel(EventDto eventDto)
		{
			return new Event(
				eventDto.Name,
				eventDto.Type,
				new Address(
					eventDto.City,
					eventDto.Street,
					eventDto.Venue),
				new EventDetails(
					eventDto.StartTime,
					eventDto.EndTime,
					eventDto.OrganizerSite,
					new Money(eventDto.Price, eventDto.Currency),
					eventDto.Description,
					eventDto.ImageFileName)
			);
		}

		private static EventDto MapModel(Event eventEntity)
		{
			return new EventDto
			{
				Id = eventEntity.Id,
				Name = eventEntity.Name,
				Type = eventEntity.Type,
				City = eventEntity.Location.City,
				Street = eventEntity.Location.Street,
				Venue = eventEntity.Location.Venue,
				StartTime = eventEntity.EventDetails.StartTime,
				EndTime = eventEntity.EventDetails.EndTime,
				OrganizerSite = eventEntity.EventDetails.OrganizerSite,
				Price = eventEntity.EventDetails.Price.Amount,
				Currency = eventEntity.EventDetails.Price.Currency,
				Description = eventEntity.EventDetails.Description,
				ImageFileName = eventEntity.EventDetails.Image,
				PotentialAttendees = eventEntity.PotentialAttendees
			};
		}
	}
}