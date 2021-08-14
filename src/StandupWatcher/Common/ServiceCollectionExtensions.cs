using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace StandupWatcher.Common
{
	public static class ServiceCollectionExtensions
	{
		public static void BindConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string key)
			where T : new()
		{
			var settings = new T();

			configuration.Bind(key, settings);
			services.AddSingleton(typeof(T), settings);
		}
	}
}