using System;
using System.Collections.Generic;
using EventCatalog.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EventCatalog.Domain.Models
{
	public class User : IdentityUser<Guid>, IEntity
	{
		public ICollection<FavoriteEvent> FavoriteEvents { get; set; }
	}
}