using System.Threading.Tasks;
using EventCatalog.Domain.Models;
using EventCatalog.WebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventCatalog.WebClient.Controllers
{
	//[Authorize(Roles = "Administrator")]
	public class ManageUsersController : Controller
	{
		private readonly UserManager<User> _userManager;

		public ManageUsersController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var currentUser = await _userManager.GetUserAsync(User);

			bool isAdmin =
				currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Administrator");

			if (!isAdmin)
			{
				return Forbid();
			}

			var admins = await _userManager.GetUsersInRoleAsync("Administrator");

			var users = await _userManager.Users.ToListAsync();

			var model = new ManageUsersViewModel
			{
				Administrators = admins,
				Users = users
			};

			return View(model);
		}
	}
}