using System;

namespace StandupWatcher.DataAccess.Models
{
	[Serializable]
	public sealed record Event : Entity
	{
		public int Id { get; init; }

		public string EventId { get; init; }

		public byte[] Data { get; init; }

		public EventStatus Status { get; init; }
	}
}