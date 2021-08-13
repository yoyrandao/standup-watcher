using System;
using System.Globalization;
using System.Linq;

using StandupWatcher.Common;
using StandupWatcher.DataAccess.Models;
using StandupWatcher.DataAccess.Repositories;
using StandupWatcher.Models;
using StandupWatcher.Processing.Notifying;
using StandupWatcher.Workers.Payload;

namespace StandupWatcher.Workers
{
	public class NotificationWorker : BaseWorker<NotificationWorkerPayload>
	{
		public NotificationWorker(
			NotificationWorkerPayload        payload,
			IBotFacade                       botFacade,
			IGenericRepository<Notification> notificationsRepository,
			IGenericRepository<Subscriber>   subscribersRepository,
			IJsonSerializer                  serializer)
			: base(payload)
		{
			_botFacade = botFacade;
			_serializer = serializer;

			_subscribersRepository = subscribersRepository;
			_notificationsRepository = notificationsRepository;
		}

		protected override void Process()
		{
			var notifications = _notificationsRepository.Get(x => !x.NotificationSent).ToList();
			var subscribers = _subscribersRepository.Get();

			notifications.ForEach(notification =>
			{
				var payload = _serializer.DeserealizeBytes<EventData[]>(notification.Data);

				foreach (var subscriber in subscribers)
				{
					foreach (var eventData in payload)
					{
						var message = ComposeNotificationMessage(eventData.Artist, eventData.Date, eventData.EventUrl);

						_botFacade.SendMessageWithPhoto(subscriber.ChatId, eventData.PictureUrl, message);
					}
				}

				_notificationsRepository.Update(notification with { NotificationSent = true });
			});

			_notificationsRepository.Save();
		}

		private static string ComposeNotificationMessage(string artist, DateTime date, string eventUrl)
		{
			return string.Format(Messages.NewEventNotification, artist,
				date.ToString("d", CultureInfo.CreateSpecificCulture("ru-RU")), eventUrl);
		}

		private readonly IBotFacade _botFacade;
		private readonly IJsonSerializer _serializer;

		private readonly IGenericRepository<Subscriber> _subscribersRepository;
		private readonly IGenericRepository<Notification> _notificationsRepository;
	}
}