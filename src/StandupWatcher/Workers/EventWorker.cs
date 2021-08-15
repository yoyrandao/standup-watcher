using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Logging;

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
			IJsonSerializer                  serializer,
			ILogger<NotificationWorker>      logger)
			: base(payload, logger)
		{
			_logger = logger;
			_serializer = serializer;
			_storeScanner = storeScanner;
			_eventRepository = eventRepository;
			_notificationRepository = notificationRepository;
		}

		protected override void Process()
		{
			var storedEvents = _eventRepository.Get(x => x.Status == EventStatus.Active).ToList();
			var actualEvents = _storeScanner.ScanForEvents().Where(x => x.Status == EventStatus.Active).ToList();

			var storedEventIds = storedEvents.Select(x => x.EventId).ToList();
			var actualEventIds = actualEvents.Select(x => x.EventId).ToList();

			var actualEventsHash = ComputeHash(actualEventIds.OrderBy(x => x));
			var storedEventsHash = ComputeHash(storedEventIds.OrderBy(x => x));

			if (actualEventsHash.SequenceEqual(storedEventsHash))
				return;

			var newEvents = actualEvents.Where(e => !storedEventIds.Contains(e.EventId)).ToList();
			var deletedEvents = storedEvents.Where(e => !actualEventIds.Contains(e.EventId)).ToList();

			if (deletedEvents.Any())
			{
				deletedEvents.ForEach(e => _eventRepository.Delete(e));
				_eventRepository.Save();
			}

			if (!newEvents.Any())
				return;

			_logger.LogInformation("Found new events. Processing...");

			var now = DateTime.Now;

			newEvents.ForEach(e => _eventRepository.Add(e with { CreationTimestamp = now, ModificationTimestamp = now }));

			PrepareNotification(newEvents, now);

			_eventRepository.Save();
			_notificationRepository.Save();

			_logger.LogInformation("New events processed.");
		}

		private void PrepareNotification(IEnumerable<Event> events, DateTime creationDate)
		{
			var notificationPayload =
				_serializer.SerializeBytes(events.Select(x => _serializer.DeserealizeBytes<EventData>(x.Data)).ToList());

			_notificationRepository.Add(new Notification
			{
				Data = notificationPayload,
				CreationTimestamp = creationDate,
				ModificationTimestamp = creationDate
			});
		}

		private static byte[] ComputeHash(IEnumerable<string> contents)
		{
			var md5 = MD5.Create();
			var content = string.Join(".", contents);

			return md5.ComputeHash(Encoding.UTF8.GetBytes(content));
		}

		private readonly IJsonSerializer _serializer;
		private readonly ILogger<NotificationWorker> _logger;

		private readonly IStoreScanner _storeScanner;
		private readonly IGenericRepository<Event> _eventRepository;
		private readonly IGenericRepository<Notification> _notificationRepository;
	}
}