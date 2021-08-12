using StandupWatcher.DataAccess.Models;
using StandupWatcher.DataAccess.Repositories;
using StandupWatcher.Workers.Payload;

namespace StandupWatcher.Workers
{
	public class EventWorker : BaseWorker<EventWorkerPayload>
	{
		public EventWorker(EventWorkerPayload payload, IGenericRepository<Event> eventRepository)
			: base(payload)
		{
			_eventRepository = eventRepository;
		}

		protected override void Process() { }

		private readonly IGenericRepository<Event> _eventRepository;
	}
}