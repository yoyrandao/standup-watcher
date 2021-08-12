using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Microsoft.Extensions.DependencyInjection;

using StandupWatcher.Common.Types;
using StandupWatcher.Workers;
using StandupWatcher.Workers.Payload;

namespace StandupWatcher.Common
{
	public class WorkerInstaller
	{
		public WorkerInstaller(IServiceCollection services)
		{
			_services = services;
			_workersToActivate = new List<Type>();
		}

		public WorkerInstaller InstallWorkers()
		{
			var serviceProvider = _services.BuildServiceProvider();
			var workersConfiguration = serviceProvider.GetService<WorkersConfiguration>();

			if (workersConfiguration is null)
				throw new SerializationException("Cannot get workers configuration block.");

			foreach (var workerConfiguration in workersConfiguration.WorkerSettings)
			{
				if (workerConfiguration.Disabled)
					continue;

				var parentType = ResolveParentType(workerConfiguration.Name);
				var (payloadType, workerType) = ResolveWorkerPayloadAndType(workerConfiguration.Name);

				var genericPayload = Activator.CreateInstance(payloadType, workerConfiguration.Interval);

				if (genericPayload is null)
					throw new ArgumentException("Cannot construct object of worker payload.");

				_services.AddSingleton(payloadType, genericPayload);
				_services.AddTransient(parentType, workerType);

				_workersToActivate.Add(parentType);
			}

			return this;
		}

		public void Run()
		{
			if (!_workersToActivate.Any())
				return;

			var serviceProvider = _services.BuildServiceProvider();

			_workersToActivate.ForEach(workerType =>
			{
				var service = serviceProvider.GetService(workerType);

				service.GetType().GetMethod("Work")?.Invoke(service, null);
			});
		}

		private static (Type, Type) ResolveWorkerPayloadAndType(string workerType)
		{
			return workerType switch
			{
				WorkerTypes.EventWorkerName => (typeof(EventWorkerPayload), typeof(EventWorker)),

				_ => throw new ArgumentOutOfRangeException(nameof(workerType), workerType, null)
			};
		}

		private static Type ResolveParentType(string workerType)
		{
			return workerType switch
			{
				WorkerTypes.EventWorkerName => typeof(BaseWorker<EventWorkerPayload>),

				_ => throw new ArgumentOutOfRangeException(nameof(workerType), workerType, null)
			};
		}

		private readonly IServiceCollection _services;
		private readonly List<Type> _workersToActivate;
	}
}