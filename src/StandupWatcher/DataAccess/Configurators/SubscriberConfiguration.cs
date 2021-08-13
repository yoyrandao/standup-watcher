using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StandupWatcher.DataAccess.Models;

namespace StandupWatcher.DataAccess.Configurators
{
	public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
	{
		public void Configure(EntityTypeBuilder<Subscriber> builder)
		{
			builder.ToTable("subscribers");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.ChatId).HasColumnName("chatid").HasColumnType("BIGINT").IsRequired();
			builder.Property(x => x.CreationTimestamp).HasColumnName("creationtimestamp").HasColumnType("TIMESTAMP").IsRequired();
			builder.Property(x => x.ModificationTimestamp).HasColumnName("modificationtimestamp").HasColumnType("TIMESTAMP").IsRequired();
		}
	}
}