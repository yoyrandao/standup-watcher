using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using StandupWatcher.Common;
using StandupWatcher.DataAccess.Models;
using StandupWatcher.DataAccess.Repositories;
using StandupWatcher.Models;
using StandupWatcher.Processing;
using StandupWatcher.Workers.Payload;

namespace StandupWatcher.Workers
{
	public class EventWorker : BaseWorker<EventWorkerPayload>
	{
		public EventWorker(
			EventWorkerPayload               payload,
			IStoreScanner                    storeScanner,
			IGenericRepository<Event>        eventRepository,
			IGenericRepository<Notification> notificationRepository,
			IJsonSerializer                  serializer)
			: base(payload)
		{
			_serializer = serializer;
			_storeScanner = storeScanner;
			_eventRepository = eventRepository;
			_notificationRepository = notificationRepository;
		}

		protected override void Process()
		{
			var storedEvents = _eventRepository.Get(x => x.Status == EventStatus.Active).ToList();
			var actualEvents = _storeScanner.ScanForEvents().Where(x => x.Status == EventStatus.Active).ToList();

			var actualEventsHash = ComputeHash(actualEvents.Select(x => x.EventId));
			var storedEventsHash = ComputeHash(storedEvents.Select(x => x.EventId));

			if (actualEventsHash.SequenceEqual(storedEventsHash))
				return;

			var storedEventIds = storedEvents.Select(x => x.EventId);
			var difference = actualEvents.Where(e => !storedEventIds.Contains(e.EventId)).ToList();

			var notificationPayload =
				_serializer.SerializeBytes(difference.Select(x => _serializer.DeserealizeBytes<EventData>(x.Data)).ToList());

			foreach (var @event in difference)
			{
				_eventRepository.Add(@event);
			}

			_notificationRepository.Add(new Notification
			{
				Data = notificationPayload,
				NotificationSent = false
			});

			_eventRepository.Save();
			_notificationRepository.Save();
		}

		private static byte[] ComputeHash(IEnumerable<string> contents)
		{
			var md5 = MD5.Create();
			var content = string.Join(".", contents);

			return md5.ComputeHash(Encoding.UTF8.GetBytes(content));
		}

		private readonly IStoreScanner _storeScanner;
		private readonly IGenericRepository<Event> _eventRepository;
		private readonly IGenericRepository<Notification> _notificationRepository;
		private readonly IJsonSerializer _serializer;
	}
}