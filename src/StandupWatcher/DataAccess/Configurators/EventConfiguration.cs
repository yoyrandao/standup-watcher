using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StandupWatcher.DataAccess.Models;


namespace StandupWatcher.DataAccess.Configurators
{
	public class EventConfiguration : IEntityTypeConfiguration<Event>
	{
		public void Configure(EntityTypeBuilder<Event> builder)
		{
			builder.ToTable("events");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Data).HasColumnName("data").HasColumnType("BYTEA").IsRequired();
			builder.Property(x => x.Status).HasColumnName("status").HasColumnType("INTEGER").IsRequired();
			builder.Property(x => x.EventId).HasColumnName("eventid").HasColumnType("VARCHAR(32)").IsRequired();
			builder.Property(x => x.CreationTimestamp).HasColumnName("creationtimestamp").HasColumnType("TIMESTAMP").IsRequired();
			builder.Property(x => x.ModificationTimestamp).HasColumnName("modificationtimestamp").HasColumnType("TIMESTAMP").IsRequired();
		}
	}
}