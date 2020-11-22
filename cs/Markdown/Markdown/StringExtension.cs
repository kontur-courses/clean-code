using System;

namespace Markdown
{
    internal static class StringExtension
    {
        internal static string GetTokenText(this string s, Token token)
        {
            if (s == null)
                throw new ArgumentException("string is null");
            return token == null ? "" : s.Substring(token.Start, token.Length);
        }
    }
}