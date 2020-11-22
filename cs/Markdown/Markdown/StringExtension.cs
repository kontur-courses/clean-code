using System;

namespace Markdown
{
    internal static class StringExtension
    {
        internal static string GetTokenText(this string s, Token token)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            return token == null ? "" : s.Substring(token.Start, token.Length);
        }
    }
}