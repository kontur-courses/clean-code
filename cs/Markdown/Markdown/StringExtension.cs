namespace Markdown
{
    internal static class StringExtension
    {
        internal static string Substring(this string s, Token token)
        {
            return token == null ? "" : s.Substring(token.Start, token.Length);
        }
    }
}