using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StandupWatcher.DataAccess.Models;


namespace StandupWatcher.DataAccess.Configurators
{
	public class SubscribedAuthorsConfiguration : IEntityTypeConfiguration<SubscribedAuthors>
	{
		public void Configure(EntityTypeBuilder<SubscribedAuthors> builder)
		{
			builder.ToTable("subscribedauthors");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.ChatId).HasColumnName("chatid").HasColumnType("BIGINT").IsRequired();
			builder.Property(x => x.StanduperName).HasColumnName("standupername").HasColumnType("VARCHAR(32)").IsRequired();
			builder.Property(x => x.CreationTimestamp).HasColumnName("creationtimestamp").HasColumnType("TIMESTAMP").IsRequired();
			builder.Property(x => x.ModificationTimestamp).HasColumnName("modificationtimestamp").HasColumnType("TIMESTAMP").IsRequired();
		}
	}
}