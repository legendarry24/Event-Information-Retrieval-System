using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventCatalog.Domain.Contracts;
using EventCatalog.Domain.Models;
using EventCatalog.Service.ApplicationServices;
using EventCatalog.Service.Models;
using EventCatalog.WebClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventCatalog.WebClient.Controllers
{
	public class EventsController : Controller
	{
		private readonly ISearchService _searchService;
		private readonly UserManager<User> _userManager;
		private readonly EditService _editService;

		public EventsController(
			ISearchService searchService,
			UserManager<User> userManager,
			EditService editService)
		{
			_searchService = searchService;
			_userManager = userManager;
			_editService = editService;
		}

		// GET: Events
		public async Task<IActionResult> Index()
		{
			var eventDtos = await _searchService.GetEventsAsync();

			var events = eventDtos.Select(MapModel);

			InitEventTypes();

			return View(events);
		}

		[HttpPost]
		public async Task<ActionResult> Index(IFormCollection collection)
		{
			var eventDtos = await _searchService.GetEventsAsync();

			// filter by city
			var city = collection["city"];

			eventDtos = eventDtos.Where(e => CityMatch(e.City, city));

			// filter by name
			var eventName = collection["eventName"];

			if (!string.IsNullOrWhiteSpace(eventName))
			{
				eventDtos = eventDtos.Where(e => e.Name.Contains(eventName, StringComparison.OrdinalIgnoreCase));
			}

			// filter by type
			var eventTypes = collection["eventTypes"];

			if (!string.IsNullOrWhiteSpace(eventTypes))
			{
				eventDtos = eventDtos.Where(e => EventTypeMatch(e.Type, eventTypes));
			}

			// filter by only favorites
			string onlyFavoritesInputValue = collection["onlyFavorites"].ToString();

			if (bool.TryParse(onlyFavoritesInputValue, out bool onlyFavorites) && onlyFavorites)
			{
				var currentUser = await _userManager.GetUserAsync(User);
				var eventDtosList = eventDtos.ToList();
				var favoriteEvents = eventDtosList.SelectMany(e => e.PotentialAttendees);

				eventDtos = eventDtosList
					.Join(favoriteEvents,
						e => e.Id,
						fe => fe.EventId,
						(e, fe) => new {Event = e, FavoriteEvent = fe})
					.Where(x => x.FavoriteEvent.UserId == currentUser.Id)
					.Select(x => x.Event);
			}

			// sort by importance criteria
			var searchCriteria = collection["searchCriteria"];

			if (searchCriteria == "relevance" || searchCriteria == "startDate")
			{
				eventDtos = eventDtos.OrderBy(e => e.StartTime).ThenBy(e => e.Price);
			}
			else
			{
				eventDtos = eventDtos.OrderBy(e => e.Price).ThenBy(e => e.StartTime);
			}

			InitEventTypes();

			var events = eventDtos.Select(MapModel);

			return View(events);
		}

		// GET: Events/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var eventDto = await _searchService.GetEventByIdAsync(id.Value);

			if (eventDto == null)
			{
				return NotFound();
			}

			var eventViewModel = MapModel(eventDto);

			return View(eventViewModel);
		}

		// GET: Events/AddToFavorites/5
		public async Task<IActionResult> AddToFavorites(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var eventDto = await _searchService.GetEventByIdAsync(id.Value);

			if (eventDto == null)
			{
				return NotFound();
			}

			var currentUser = await _userManager.GetUserAsync(User);

			await _editService.AddEventToFavoritesAsync(
				new EventUserIdentity(eventDto.Id, currentUser.Id));

			return RedirectToAction(nameof(Details), new { id = eventDto.Id});
		}

		private void InitEventTypes()
		{
			var eventTypes = (EventType[])Enum.GetValues(typeof(EventType));

			ViewBag.EventTypes = eventTypes.Where(type => type != EventType.Unknown);
		}

		private static bool CityMatch(string city, string chosenCity)
		{
			if (string.IsNullOrWhiteSpace(chosenCity) || chosenCity == "all")
			{
				return true;
			}

			return city == chosenCity;
		}

		private static bool EventTypeMatch(EventType eventType, IEnumerable<string> eventTypes)
		{
			var eventTypesList = ParseEventTypes(eventTypes);

			return eventTypesList.Any(e => e == eventType);
		}

		private static IEnumerable<EventType> ParseEventTypes(IEnumerable<string> eventTypes)
		{
			var eventTypesList = new List<EventType>();
			eventTypes = eventTypes.ToString().Split(',');
			foreach (string type in eventTypes)
			{
				if (Enum.TryParse(type, out EventType eventType))
				{
					eventTypesList.Add(eventType);
				}
			}

			return eventTypesList;
		}

		private static EventViewModel MapModel(EventDto eventDto)
		{
			return new EventViewModel
			{
				Id = eventDto.Id,
				Name = eventDto.Name,
				Type = eventDto.Type,
				City = eventDto.City,
				Street = eventDto.Street,
				Venue = eventDto.Venue,
				StartTime = eventDto.StartTime,
				EndTime = eventDto.EndTime,
				OrganizerSite = eventDto.OrganizerSite,
				Price = eventDto.Price,
				Currency = eventDto.Currency,
				Description = eventDto.Description,
				ImageName = eventDto.ImageFileName
			};
		}
	}
}