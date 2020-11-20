namespace Markdown
{
    public static class StringExtension
    {
        public static string Substring(this string s, Token token)
        {
            return s.Substring(token.Start, token.Length);
        }
    }
}