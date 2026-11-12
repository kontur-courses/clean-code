using Markdown;
using Markdown.Models;
using Markdown.Models.Interfaces;
using System;


namespace MarkdownUnitTests.Extensions
{
    public static class TokenExtensions
    {
        public static bool IsTag(this IToken token) => token is TagToken;
    }
}
