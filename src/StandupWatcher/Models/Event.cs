﻿using System;

namespace StandupWatcher.Models
{
	public record EventData
	{
		public string Artist { get; init; }

		public string PictureUrl { get; init; }

		public string EventUrl { get; init; }

		public DateTime Date { get; init; }
	}
}