using System.Text.RegularExpressions;


namespace StandupWatcher.Common
{
	public static class StringExtensions
	{
		public static string RegexReplace(this string @string, Regex regex, string replacement)
		{
			return Regex.Replace(@string, regex.ToString(), replacement);
		}
	}
}