using System;
using EventCatalog.DataAccess;
using EventCatalog.Domain.Models;
using EventCatalog.Service.ApplicationServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventCatalog.WebClient
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<EventCatalogContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("LocalConnection"),
					b => b.MigrationsAssembly("EventCatalog.DataAccess"));
			});

			services.AddDefaultIdentity<User>()
				.AddRoles<IdentityRole<Guid>>()
				.AddEntityFrameworkStores<EventCatalogContext>();

			//services.AddIdentity<User, IdentityRole>(options =>
			//{
			//	options.Password.RequiredLength = 5;
			//	options.Password.RequireNonAlphanumeric = false;
			//	options.Password.RequireLowercase = false;
			//	options.Password.RequireUppercase = false;
			//	options.Password.RequireDigit = false;

			//	options.User.RequireUniqueEmail = true;
			//})
			//	.AddEntityFrameworkStores<EventCatalogContext>()
			//	.AddDefaultTokenProviders();

			//services.AddTransient<IEmailSender, EmailSender>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddScoped<ISearchService, SearchService>();
			services.AddScoped(s => new CreationService());
			services.AddScoped(s => new EditService());
			services.AddScoped(s => new DeletionService());
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}