using System;

using StandupWatcher.Common.Types;


namespace StandupWatcher.Workers.Payload
{
	public class NotificationWorkerPayload : WorkerPayload
	{
		public NotificationWorkerPayload(TimeSpan interval) : base(interval, WorkerTypes.NotificationWorkerName) { }
	}
}