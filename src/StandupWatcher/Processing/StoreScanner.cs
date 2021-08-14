using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

using AngleSharp;
using AngleSharp.Dom;

using StandupWatcher.Common;
using StandupWatcher.DataAccess.Models;
using StandupWatcher.Models;


namespace StandupWatcher.Processing
{
	public class StoreScanner : IStoreScanner
	{
		public StoreScanner(IContentProvider contentProvider, IJsonSerializer serializer, string targetUrl)
		{
			_targetUrl = targetUrl;
			_serializer = serializer;
			_contentProvider = contentProvider;

			_browsingContext = BrowsingContext.New(Configuration.Default.WithCss());
		}

		#region Implementation of IStoreScanner

		public List<Event> ScanForEvents()
		{
			var pageContent = _contentProvider.GetPageContent(_targetUrl);

			var document = _browsingContext
				.OpenAsync(request => request.Content(pageContent))
				.GetAwaiter().GetResult();

			var events = new List<Event>();
			var pageEvents = document.GetElementsByClassName("event");

			foreach (var @event in pageEvents)
			{
				if (@event.Id is null)
					continue;

				var eventContent = document.GetElementById(@event.Id);

				var isOnSoldOut = IsOnSoldOut(eventContent);
				var eventData = GetEventData(eventContent);

				events.Add(new Event
				{
					EventId = @event.Id,
					Status = isOnSoldOut ? EventStatus.SoldOut : EventStatus.Active,
					Data = _serializer.SerializeBytes(eventData)
				});
			}

			return events;
		}

		#endregion

		private static bool IsOnSoldOut(IElement nodeElement)
		{
			var soldOutBanners = nodeElement.QuerySelectorAll(".evo_soldout");

			return soldOutBanners.Any();
		}

		private EventData GetEventData(IElement nodeElement)
		{
			var eventSchemaBlock = nodeElement
				?.GetElementsByClassName("evo_event_schema").SingleOrDefault()
				?.GetElementsByTagName("script").SingleOrDefault();

			var eventSchemaContent = _serializer.Deserialize<EventSchema>(eventSchemaBlock?.InnerHtml);

			if (eventSchemaContent is null)
				throw new SerializationException("Cannot deserialize event schema from resource.");

			return new EventData
			{
				EventUrl = eventSchemaContent.Url,
				Date = eventSchemaContent.StartDate,
				PictureUrl = eventSchemaContent.Image,
				Artist = eventSchemaContent.Description
					.Replace("<!-- wp:paragraph -->", string.Empty)
					.Replace("<!-- /wp:paragraph -->", string.Empty)
					.Replace("<p>", string.Empty)
					.Replace("</p>", string.Empty)
					.Trim()
					.RegexReplace(new Regex("\\s{2,}"), ". ")
			};
		}

		private readonly string _targetUrl;

		private readonly IJsonSerializer _serializer;

		private readonly IContentProvider _contentProvider;
		private readonly IBrowsingContext _browsingContext;
	}
}