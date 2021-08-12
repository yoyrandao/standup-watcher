using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using StandupWatcher.Common;
using StandupWatcher.Common.Types;
using StandupWatcher.DataAccess;
using StandupWatcher.DataAccess.Repositories;

namespace StandupWatcher
{
	public static class Startup
	{
		public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			_configuration = context.Configuration;

			services.BindConfiguration<WorkersConfiguration>(_configuration, "workers");

			services.AddDbContext<DatabaseContext>(
				options => options.UseNpgsql(_configuration.GetConnectionString("watcherDatabase")));
			services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			new WorkerInstaller(services).InstallWorkers().Run();
		}

		private static IConfiguration _configuration;
	}
}