using System;


namespace StandupWatcher.Common.Types
{
	[Serializable]
	public record ScannerConfiguration
	{
		public string StoreUrl { get; init; }
	}
}