using System;

namespace StandupWatcher.Workers.Payload
{
	public class NotificationWorkerPayload : WorkerPayload
	{
		public NotificationWorkerPayload(TimeSpan interval) : base(interval) { }
	}
}