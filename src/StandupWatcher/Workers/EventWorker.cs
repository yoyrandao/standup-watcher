using StandupWatcher.DataAccess.Models;
using StandupWatcher.DataAccess.Repositories;
using StandupWatcher.Processing;
using StandupWatcher.Workers.Payload;

namespace StandupWatcher.Workers
{
	public class EventWorker : BaseWorker<EventWorkerPayload>
	{
		public EventWorker(EventWorkerPayload payload, IStoreScanner aboba)
			: base(payload)
		{
			_aboba = aboba;
		}

		protected override void Process()
		{
			_aboba.Scan();
		}

		private readonly IStoreScanner _aboba;
	}
}