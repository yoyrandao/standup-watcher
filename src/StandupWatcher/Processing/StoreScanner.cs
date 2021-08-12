using System.Collections.Generic;
using System.Linq;

using AngleSharp;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

using StandupWatcher.DataAccess.Models;
using StandupWatcher.Models;

namespace StandupWatcher.Processing
{
	public class StoreScanner : IStoreScanner
	{
		public StoreScanner(IContentProvider contentProvider, string targetUrl)
		{
			_targetUrl = targetUrl;
			_contentProvider = contentProvider;

			_browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader().WithCss());
		}

		#region Implementation of IStoreScanner

		public List<Event> Scan()
		{
			var pageContent = _contentProvider.GetPageContent(_targetUrl);

			var document = _browsingContext
				.OpenAsync(request => request.Content(pageContent))
				.GetAwaiter().GetResult();

			var events = new List<Event>();
			var pageEvents = document.GetElementsByClassName("event");

			foreach (var @event in pageEvents)
			{
				var isOnSoldOut = IsOnSoldOut(document, @event.Id);
				var image = GetPictureUrl(document, @event.Id);

				events.Add(new Event
				{
					EventId = @event.Id,
					Status = isOnSoldOut ? EventStatus.SoldOut : EventStatus.Active
				});
			}

			return events;
		}

		#endregion

		private static bool IsOnSoldOut(IDocument document, string eventId)
		{
			var eventContent = document.GetElementById(eventId);

			if (eventContent is null)
				return false;

			var soldOutBanners = eventContent.QuerySelectorAll(".evo_soldout");

			return soldOutBanners.Any();
		}

		private static string GetPictureUrl(IDocument document, string eventId)
		{
			var eventContent = document.GetElementById(eventId);
			var pictureBox = (IHtmlElement)eventContent?.QuerySelectorAll(".evo_boxtop").Single();

			return null;
		}

		private readonly string _targetUrl;

		private readonly IContentProvider _contentProvider;
		private readonly IBrowsingContext _browsingContext;
	}
}