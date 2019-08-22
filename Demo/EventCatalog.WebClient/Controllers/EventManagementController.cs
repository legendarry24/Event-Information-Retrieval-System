using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventCatalog.Service.ApplicationServices;
using EventCatalog.Service.Models;
using EventCatalog.WebClient.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EventCatalog.WebClient.Controllers
{
	public class EventManagementController : Controller
	{
		private readonly ISearchService _searchService;
		private readonly CreationService _creationService;
		private readonly EditService _editService;
		private readonly DeletionService _deletionService;
		private readonly IHostingEnvironment _hostingEnvironment;

		public EventManagementController(
			ISearchService searchService,
			DeletionService deletionService,
			CreationService creationService,
			EditService editService,
			IHostingEnvironment hostingEnvironment)
		{
			_searchService = searchService;
			_deletionService = deletionService;
			_creationService = creationService;
			_editService = editService;
			_hostingEnvironment = hostingEnvironment;
		}

		// GET: EventManagement
		public async Task<IActionResult> Index()
		{
			var eventDtos = await _searchService.GetEventsAsync();

			var events = eventDtos.Select(MapModel);

			return View(events);
		}

		// GET: EventManagement/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: EventManagement/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EventInputModel eventModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			decimal price = 0;

			if (!string.IsNullOrWhiteSpace(eventModel.Price) &&
			    !decimal.TryParse(eventModel.Price, NumberStyles.Currency, CultureInfo.InvariantCulture, out price))
			{
				return View();
			}

			EventDto eventDto = MapModel(eventModel);
			eventDto.Price = price;

			if (eventModel.Image != null)
			{
				eventDto.ImageFileName = await SaveUploadedImage(eventModel.Image);
			}

			await _creationService.CreateEventAsync(eventDto);

			return RedirectToAction(nameof(Index));
		}

		// GET: EventManagement/Edit/5
		public async Task<IActionResult> Edit(Guid id)
		{
			var eventDto = await _searchService.GetEventByIdAsync(id);

			if (eventDto == null)
			{
				return NotFound();
			}

			var eventViewModel = MapModel(eventDto);

			return View(eventViewModel);
		}

		// POST: EventManagement/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, EventInputModel eventModel)
		{
			if (id != eventModel.Id)
			{
				return NotFound();
			}

			var eventDto = await _searchService.GetEventByIdAsync(id);

			if (eventDto == null)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return View(MapModel(eventDto));
			}

			if (!decimal.TryParse(eventModel.Price, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal price))
			{
				return View(MapModel(eventDto));
			}

			eventDto.Name = eventModel.Name;
			eventDto.Type = eventModel.Type;
			eventDto.City = eventModel.City;
			eventDto.Street = eventModel.Street;
			eventDto.Venue = eventModel.Venue;
			eventDto.StartTime = eventModel.StartTime;
			eventDto.EndTime = eventModel.EndTime;
			eventDto.OrganizerSite = eventModel.OrganizerSite;
			eventDto.Price = price;
			eventDto.Currency = eventModel.Currency;
			eventDto.Description = eventModel.Description;

			if (eventModel.Image != null)
			{
				eventDto.ImageFileName = await SaveUploadedImage(eventModel.Image);
			}

			await _editService.UpdateEventAsync(eventDto);

			return RedirectToAction(nameof(Index));
		}

		// GET: EventManagement/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
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

		// POST: EventManagement/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var eventDto = await _searchService.GetEventByIdAsync(id);

			if (eventDto == null)
			{
				return NotFound();
			}

			await _deletionService.DeleteEventAsync(id);

			return RedirectToAction(nameof(Index));
		}

		private async Task<string> SaveUploadedImage(IFormFile image)
		{
			string imagePath = Path.Combine(
				_hostingEnvironment.WebRootPath + @"\images\eventImages",
				image.FileName);

			using (var stream = new FileStream(imagePath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}

			return image.FileName;
		}

		private static EventDto MapModel(EventInputModel eventInputModel)
		{
			return new EventDto
			{
				Name = eventInputModel.Name,
				Type = eventInputModel.Type,
				City = eventInputModel.City,
				Street = eventInputModel.Street,
				Venue = eventInputModel.Venue,
				StartTime = eventInputModel.StartTime,
				EndTime = eventInputModel.EndTime,
				OrganizerSite = eventInputModel.OrganizerSite,
				Currency = eventInputModel.Currency,
				Description = eventInputModel.Description
			};
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