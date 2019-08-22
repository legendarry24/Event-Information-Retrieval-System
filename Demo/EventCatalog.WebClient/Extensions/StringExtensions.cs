using System;

namespace EventCatalog.WebClient.Extensions
{
	public static class StringExtensions
	{
		public static bool Contains(this string source, string toCheck, StringComparison comparison)
		{
			return source?.IndexOf(toCheck, comparison) >= 0;
		}
	}
}