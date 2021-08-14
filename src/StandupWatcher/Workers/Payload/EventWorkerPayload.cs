using System;

using StandupWatcher.Common.Types;


namespace StandupWatcher.Workers.Payload
{
	public class EventWorkerPayload : WorkerPayload
	{
		public EventWorkerPayload(TimeSpan interval) : base(interval, WorkerTypes.EventWorkerName) { }
	}
}