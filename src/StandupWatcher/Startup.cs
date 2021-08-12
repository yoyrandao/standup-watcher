using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using StandupWatcher.Common;
using StandupWatcher.Common.Types;

namespace StandupWatcher
{
	public static class Startup
	{
		public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			_configuration = context.Configuration;

			services.BindConfiguration<WorkersConfiguration>(_configuration, "workers");

			new WorkerInstaller(services)
				.InstallWorkers()
				.Run();
		}

		private static IConfiguration _configuration;
	}
}