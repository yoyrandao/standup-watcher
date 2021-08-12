using System;

using StandupWatcher.Workers.Payload;

namespace StandupWatcher.Workers
{
	public class EventWorker : BaseWorker<EventWorkerPayload>
	{
		public EventWorker(EventWorkerPayload payload) : base(payload)
		{
			Console.WriteLine("asda");
		}

		protected override void Process()
		{
			Console.WriteLine("hello");
		}
	}
}