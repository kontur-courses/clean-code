using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal static class TokenListExtension
    {
        public static List<Token> ValidateObjectTokens(this List<Token> tokens)
        {
            return MarkdownTokenValidator.ValidateObjectTokens(tokens);
        }

        public static List<Token> ResolveObjectIntersections(this List<Token> tokens)
        {
            return MarkdownObjectIntersectionResolver.ResolveObjectTokenIntersections(tokens);
        }
    }
}