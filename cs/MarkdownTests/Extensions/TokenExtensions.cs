using Markdown;
using Markdown.Models;
using Markdown.Models.Interfaces;
using System;


namespace MarkdownUnitTests.Extensions
{
    public static class TokenExtensions
    {
        public static bool IsTag(this IToken token) => token is TagToken;

        public static bool IsText(this IToken token) => token is TextToken;

        public static TagToken AsTag(this IToken token) => (TagToken)token;

        public static TextToken AsText(this IToken token) => (TextToken)token;
    }
}
