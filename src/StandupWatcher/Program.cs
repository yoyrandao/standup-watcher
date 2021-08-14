using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;


namespace StandupWatcher
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, configurationBuilder) =>
				{
					configurationBuilder.Sources.Clear();

					configurationBuilder
						.SetBasePath(context.HostingEnvironment.ContentRootPath)
						.AddYamlFile("appsettings.yaml", false, true)
						.AddYamlFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.yaml", true, true)
						.AddEnvironmentVariables();

					if (args != null)
						configurationBuilder.AddCommandLine(args);
				})
				.ConfigureServices(Startup.ConfigureServices)
				.UseSerilog();
	}
}