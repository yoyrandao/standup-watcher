using System;


namespace StandupWatcher.DataAccess.Models
{
	[Serializable]
	public sealed record Subscriber : Entity
	{
		public long ChatId { get; init; }
	}
}