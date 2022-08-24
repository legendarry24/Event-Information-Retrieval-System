using System;
using EventCatalog.DataAccess.EntityConfigurations;
using EventCatalog.Domain.Models;
using EventCatalog.Domain.Models.EventAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventCatalog.DataAccess
{
	public class EventCatalogContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		public EventCatalogContext(DbContextOptions options)
			: base(options)
		{ }

		public DbSet<Event> Events { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new EventEntityConfiguration());
			modelBuilder.ApplyConfiguration(new FavoriteEventEntityConfiguration());
			
			base.OnModelCreating(modelBuilder);
		}
	}
}
