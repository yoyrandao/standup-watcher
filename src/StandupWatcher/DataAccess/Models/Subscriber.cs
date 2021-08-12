using System;

namespace StandupWatcher.DataAccess.Models
{
	[Serializable]
	public sealed record Subscriber : Entity
	{
		public int Id { get; init; }

		public long ChatId { get; init; }
	}
}