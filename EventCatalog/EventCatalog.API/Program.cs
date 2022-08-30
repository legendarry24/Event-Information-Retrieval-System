using System.Text.Json.Serialization;
using EventCatalog.DataAccess;
using EventCatalog.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EventCatalog.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddDbContext<EventCatalogContext>(options =>
			{
				// use LocalConnection when running the app on IIS
				options.UseSqlServer(builder.Configuration.GetConnectionString("DockerConnection"),
					b => b.MigrationsAssembly("EventCatalog.DataAccess"));
			});

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.AddControllers()
				.AddJsonOptions(options =>
				{
					// in Newtonsoft.Json it was: options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
				});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}