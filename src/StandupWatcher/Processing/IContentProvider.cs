namespace StandupWatcher.Processing
{
	public interface IContentProvider
	{
		string GetPageContent(string url);
	}
}