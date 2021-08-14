using System;


namespace StandupWatcher.Models
{
	[Serializable]
	public record EventSchema
	{
		public string Name { get; init; }

		public string Url { get; init; }

		public DateTime StartDate { get; init; }

		public DateTime EndDate { get; init; }

		public string Image { get; init; }

		public string Description { get; init; }
	}
}