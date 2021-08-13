using System.Collections.Generic;

using StandupWatcher.DataAccess.Models;

namespace StandupWatcher.Processing
{
	public interface IStoreScanner
	{
		List<Event> ScanForEvents();
	}
}