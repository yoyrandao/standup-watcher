using System;

namespace StandupWatcher.Common.Types
{
	[Serializable]
	public record WorkerSettings
	{
		public string Name { get; init; }

		public bool Disabled { get; init; }

		public TimeSpan Interval { get; init; }
	}
}