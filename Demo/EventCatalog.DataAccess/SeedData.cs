using System;
using System.Linq;
using System.Threading.Tasks;
using EventCatalog.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventCatalog.DataAccess
{
	public static class SeedData
	{
		public static async Task InitializeAsync(IServiceProvider services)
		{
			var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			await EnsureRolesAsync(roleManager);

			var userManager = services.GetRequiredService<UserManager<User>>();

			await EnsureTestAdminAsync(userManager);
		}

		private static async Task EnsureRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
		{
			bool alreadyExists = await roleManager.RoleExistsAsync("Administrator");

			if (alreadyExists) return;

			await roleManager.CreateAsync(new IdentityRole<Guid>("Administrator"));
		}

		private static async Task EnsureTestAdminAsync(UserManager<User> userManager)
		{
			var testAdmin = await userManager.Users
				.Where(x => x.UserName == "admin@event.catalog")
				.SingleOrDefaultAsync();

			if (testAdmin != null) return;

			testAdmin = new User
			{
				UserName = "admin@event.catalog",
				Email = "admin@event.catalog"
			};

			await userManager.CreateAsync(testAdmin, "Password1!");

			await userManager.AddToRoleAsync(testAdmin, "Administrator");
		}
	}
}