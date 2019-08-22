using EventCatalog.DataAccess;
using EventCatalog.Domain.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventCatalog.WebService
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
			services.AddDbContext<EventCatalogContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("LocalConnection"),
					b => b.MigrationsAssembly("EventCatalog.DataAccess"));
			});

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddMvc()
				.AddJsonOptions(options =>
					{
						options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					})
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}