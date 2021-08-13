using System;

namespace StandupWatcher.Common.Types
{
	[Serializable]
	public record BotConfiguration
	{
		public string AccessToken { get; init; }
	}
}