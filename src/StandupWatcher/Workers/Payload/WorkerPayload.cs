using System;


namespace StandupWatcher.Workers.Payload
{
	public class WorkerPayload
	{
		protected WorkerPayload(TimeSpan interval, string name)
		{
			Name = name;
			Interval = interval;
		}

		public TimeSpan Interval { get; }

		public string Name { get; }
	}
}