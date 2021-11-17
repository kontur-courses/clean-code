namespace Markdown
{
    public static class StringExtensions
    {
        public static Token[] ParseIntoTokens(this string text)
        {
            return new TokenParser().Parse(text);
        }
    }
}