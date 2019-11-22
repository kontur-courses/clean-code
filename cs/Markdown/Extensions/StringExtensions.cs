namespace Markdown
{
    public static class StringExtensions
    {
        internal static string RemoveEscapeSymbols(this string input) => input.Replace(@"\", "");
    }
}