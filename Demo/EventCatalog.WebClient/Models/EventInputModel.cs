using System;
using System.ComponentModel.DataAnnotations;
using EventCatalog.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventCatalog.WebClient.Models
{
	public class EventInputModel
	{
		[HiddenInput]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		public EventType Type { get; set; }

		[Required]
		[MaxLength(50)]
		public string City { get; set; }

		[MaxLength(100)]
		public string Street { get; set; }

		[MaxLength(100)]
		public string Venue { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		[Required]
		[MaxLength(100)]
		public string OrganizerSite { get; set; }

		public string Price { get; set; }

		public Currency Currency { get; set; }

		public string Description { get; set; }

		public IFormFile Image { get; set; }
	}
}