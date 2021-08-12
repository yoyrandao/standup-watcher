using System;

namespace StandupWatcher.DataAccess.Models
{
	[Serializable]
	public sealed record Notification : Entity
	{
		public int Id { get; init; }

		public byte[] Data { get; init; }

		public bool NotificationSent { get; init; }
	}
}