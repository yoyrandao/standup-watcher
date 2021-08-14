using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using StandupWatcher.Common;
using StandupWatcher.Common.Types;
using StandupWatcher.DataAccess;
using StandupWatcher.DataAccess.Repositories;
using StandupWatcher.Processing;
using StandupWatcher.Processing.Notifying;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;


namespace StandupWatcher
{
	public static class Startup
	{
		public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			_configuration = context.Configuration;
			_tokenSource = new CancellationTokenSource();

			services.BindConfiguration<BotConfiguration>(_configuration, "bot");
			services.BindConfiguration<WorkersConfiguration>(_configuration, "workers");
			services.BindConfiguration<ScannerConfiguration>(_configuration, "scanner");

			ConfigureLogic(services);
			ConfigureDatabase(services);

			ConfigureBot(services);

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
			/* Common */
			services.AddTransient<IJsonSerializer, JsonSerializer>();

			/* Content Parsing */
			services.AddTransient<IContentProvider, ContentProvider>();

			services.AddTransient<IStoreScanner, StoreScanner>(
				x => new StoreScanner(
					x.GetService<IContentProvider>(),
					x.GetService<IJsonSerializer>(),
					x.GetService<ScannerConfiguration>()?.StoreUrl));
		}

		private static void ConfigureBot(IServiceCollection services)
		{
			services.AddTransient<IBotFacade, BotFacade>();

			services.AddSingleton<ITelegramBotClient, TelegramBotClient>(
				x => new TelegramBotClient(x.GetService<BotConfiguration>()?.AccessToken));

			var serviceProvider = services.BuildServiceProvider();

			var bot = serviceProvider.GetService<ITelegramBotClient>();
			var updateHandler = serviceProvider.GetService<IBotFacade>();

			bot!.StartReceiving(new DefaultUpdateHandler(updateHandler!.HandleMessages, updateHandler.HandleError), _tokenSource.Token);
		}

		private static IConfiguration _configuration;
		private static CancellationTokenSource _tokenSource;
	}
}