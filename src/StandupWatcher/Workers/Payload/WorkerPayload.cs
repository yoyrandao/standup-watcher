using System;


namespace StandupWatcher.Workers.Payload
{
	public class WorkerPayload
	{
		protected WorkerPayload(TimeSpan interval)
		{
			Interval = interval;
		}

		public TimeSpan Interval { get; private init; }
	}
}