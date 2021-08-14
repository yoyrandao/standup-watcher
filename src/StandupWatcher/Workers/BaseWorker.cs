using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using StandupWatcher.Common;
using StandupWatcher.Workers.Payload;


namespace StandupWatcher.Workers
{
	public class BaseWorker<T> where T : WorkerPayload
	{
		protected BaseWorker(T payload, ILogger logger)
		{
			_payload = payload;
			_logger = logger;
		}

		public async Task Work(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				var workerName = _payload.Name.Capitalize();

				_logger.LogInformation($"{workerName} started processing.");

				try
				{
					Process();
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Error occured.");
				}

				_logger.LogInformation($"{workerName} finished processing.");

				await Task.Delay(_payload.Interval, cancellationToken);
			}
		}

		protected virtual void Process() { }

		private readonly WorkerPayload _payload;
		private readonly ILogger _logger;
	}
}