using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StandupWatcher.DataAccess.Models;

namespace StandupWatcher.DataAccess.Configurators
{
	public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
	{
		public void Configure(EntityTypeBuilder<Subscriber> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.ChatId).HasColumnName("chatid").HasColumnType("BIGINT").IsRequired();
		}
	}
}