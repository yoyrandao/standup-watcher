using System.Collections.Generic;

using StandupWatcher.DataAccess.Models;
using StandupWatcher.Models;

namespace StandupWatcher.Processing
{
	public interface IStoreScanner
	{
		List<Event> Scan();
	}
}