using System;


namespace StandupWatcher.Workers.Payload
{
	public class EventWorkerPayload : WorkerPayload
	{
		public EventWorkerPayload(TimeSpan interval) : base(interval) { }
	}
}