namespace Markdown
{
    public static class StringExtension
    {
        public static string ClearFromSymbol(this string str, char symbol)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace(new string(new[] { symbol }), string.Empty);
        }
    }
}