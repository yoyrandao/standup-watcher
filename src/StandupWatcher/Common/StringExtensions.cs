using System.Text.RegularExpressions;


namespace StandupWatcher.Common
{
	public static class StringExtensions
	{
		public static string RegexReplace(this string @string, Regex regex, string replacement)
		{
			return Regex.Replace(@string, regex.ToString(), replacement);
		}

		public static string Capitalize(this string @string)
		{
			if (@string.Length == 0)
				return string.Empty;

			return @string[0].ToString().ToUpper() + @string[1..];
		}
	}
}