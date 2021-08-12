using System.IO;
using System.Net;

namespace StandupWatcher.Processing
{
	public class ContentProvider : IContentProvider
	{
		public string GetPageContent(string url)
		{
			const int requestTimeout = 4000;

			var request = WebRequest.Create(url);
			request.Timeout = requestTimeout;

			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();

			using var reader = new StreamReader(responseStream!);

			return reader.ReadToEnd();
		}
	}
}