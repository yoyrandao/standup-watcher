using System;


namespace StandupWatcher.DataAccess.Models
{
	/* Marker record for Entity Framework Models and Repositories */
	public record Entity
	{
		public int Id { get; init; }

		public DateTime CreationTimestamp { get; set; }

		public DateTime ModificationTimestamp { get; set; }
	}
}