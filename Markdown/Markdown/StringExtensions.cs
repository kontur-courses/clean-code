namespace Markdown
{
	public static class StringExtensions
	{
		public static string RemoveScreenCharacters(this string text) => text.Replace("\\", "");

	}
}
