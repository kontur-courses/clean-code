using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal static class TokenListExtension
    {
        public static List<Token> ValidateObjectTokens(this List<Token> tokens)
        {
            return MarkdownTokenAnalyzer.ValidateObjectTokens(tokens);
        }

        public static List<Token> ResolveObjectIntersections(this List<Token> tokens)
        {
            return MarkdownTokenAnalyzer.ResolveObjectTokenIntersections(tokens);
        }
    }
}