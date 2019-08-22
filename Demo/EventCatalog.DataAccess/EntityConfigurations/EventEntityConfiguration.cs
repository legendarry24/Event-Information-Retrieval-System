using EventCatalog.Domain.Contracts;
using EventCatalog.Domain.Models.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventCatalog.DataAccess.EntityConfigurations
{
	public class EventEntityConfiguration : IEntityTypeConfiguration<Event>
	{
		public void Configure(EntityTypeBuilder<Event> builder)
		{
			builder.ToTable(nameof(Event));

			//builder.Property(p => p.Id)
			//	.ValueGeneratedOnAdd();

			builder.Property(p => p.Name)
				.IsRequired()
				.IsUnicode()
				.HasMaxLength(100);

			builder.Property(p => p.Type)
				.IsRequired()
				.HasColumnType("int")
				.HasDefaultValue(EventType.Unknown);

			builder.OwnsOne(p => p.EventDetails, cb =>
			{
				cb.OwnsOne(c => c.Price)
					.Property(p => p.Amount)
					.HasColumnType("money");

				cb.Property(p => p.StartTime)
					.IsRequired();

				cb.Property(p => p.OrganizerSite)
					.IsRequired()
					.IsUnicode()
					.HasMaxLength(100);
			});

			builder.OwnsOne(p => p.Location, cb =>
			{
				cb.Property(p => p.City)
					.IsRequired()
					.IsUnicode()
					.HasMaxLength(50);

				cb.Property(p => p.Street)
					.IsUnicode()
					.HasMaxLength(100);

				cb.Property(p => p.Venue)
					.IsUnicode()
					.HasMaxLength(100);
			});
		}
	}
}