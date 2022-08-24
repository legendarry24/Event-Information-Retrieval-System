using EventCatalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventCatalog.DataAccess.EntityConfigurations
{
	public class FavoriteEventEntityConfiguration : IEntityTypeConfiguration<FavoriteEvent>
	{
		public void Configure(EntityTypeBuilder<FavoriteEvent> builder)
		{
			builder.HasKey(fe => new { fe.EventId, fe.UserId });

			builder.HasOne(fe => fe.Event)
				.WithMany(b => b.PotentialAttendees)
				.HasForeignKey(bu => bu.EventId);

			builder.HasOne(fe => fe.User)
				.WithMany(u => u.FavoriteEvents)
				.HasForeignKey(bu => bu.UserId);
		}
	}
}