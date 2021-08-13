using Microsoft.EntityFrameworkCore;

using StandupWatcher.DataAccess.Configurators;

namespace StandupWatcher.DataAccess
{
	public sealed class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		#region Overriding of DbContext

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasDefaultSchema("public");

			builder.ApplyConfiguration(new EventConfiguration());
			builder.ApplyConfiguration(new SubscriberConfiguration());
			builder.ApplyConfiguration(new NotificationConfiguration());
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSnakeCaseNamingConvention();
		}

		#endregion
	}
}