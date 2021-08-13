using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using StandupWatcher.Common;
using StandupWatcher.Common.Types;
using StandupWatcher.DataAccess;
using StandupWatcher.DataAccess.Repositories;
using StandupWatcher.Processing;

namespace StandupWatcher
{
	public static class Startup
	{
		public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			_configuration = context.Configuration;

			services.BindConfiguration<WorkersConfiguration>(_configuration, "workers");
			services.BindConfiguration<ScannerConfiguration>(_configuration, "scanner");

			ConfigureLogic(services);
			ConfigureDatabase(services);

			new WorkerInstaller(services).Install().Run();
		}

		private static void ConfigureDatabase(IServiceCollection services)
		{
			services.AddDbContext<DatabaseContext>(
				options => options.UseNpgsql(_configuration.GetConnectionString("watcherDatabase")));

			services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
		}

		private static void ConfigureLogic(IServiceCollection services)
		{
			services.AddTransient<IJsonSerializer, JsonSerializer>();
			services.AddTransient<IContentProvider, ContentProvider>();

			services.AddTransient<IStoreScanner, StoreScanner>(
				x => new StoreScanner(
					x.GetService<IContentProvider>(),
					x.GetService<IJsonSerializer>(),
					x.GetService<ScannerConfiguration>()?.StoreUrl));
		}

		private static IConfiguration _configuration;
	}
}