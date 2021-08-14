using System;


namespace StandupWatcher.Common.Types
{
	[Serializable]
	public record WorkersConfiguration
	{
		public WorkerSettings[] WorkerSettings { get; init; }
	}
}