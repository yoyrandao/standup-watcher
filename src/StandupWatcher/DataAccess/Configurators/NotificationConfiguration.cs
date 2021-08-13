using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StandupWatcher.DataAccess.Models;

namespace StandupWatcher.DataAccess.Configurators
{
	public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			builder.ToTable("notifications");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Data).HasColumnName("data").HasColumnType("BYTEA").IsRequired();
			builder.Property(x => x.NotificationSent).HasColumnName("notificationsent").HasColumnType("BOOLEAN").IsRequired();
		}
	}
}