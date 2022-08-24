using System.Collections.Generic;
using EventCatalog.Domain.Models;

namespace EventCatalog.WebClient.Models
{
	public class ManageUsersViewModel
	{
		public IEnumerable<User> Administrators { get; set; }

		public IEnumerable<User> Users { get; set; }
	}
}