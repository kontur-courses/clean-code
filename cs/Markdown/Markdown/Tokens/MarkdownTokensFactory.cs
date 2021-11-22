using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens
{
    public static class MarkdownTokensFactory
    {
        public static IToken Italic()
        {
            return new Token
            {
                Pattern = new PairedTokenPattern("_") {ForbiddenChildren = new List<TagType> {TagType.Bold}},
                TagConverter = new HtmlTagConverter
                {
                    HtmlOpenTag = "<em>",
                    HtmlCloseTag = "</em>",
                    TrimFromStartCount = 1,
                    TrimFromEndCount = 1
                },
                TagType = TagType.Italic
            };
        }

        public static IToken Bold()
        {
            return new Token
            {
                Pattern = new PairedTokenPattern("__"),
                TagConverter = new HtmlTagConverter
                {
                    HtmlOpenTag = "<strong>",
                    HtmlCloseTag = "</strong>",
                    TrimFromStartCount = 2,
                    TrimFromEndCount = 2
                },
                TagType = TagType.Bold
            };
        }

        public static IToken Header()
        {
            return new Token
            {
                Pattern = new HeaderTokenPattern(),
                TagConverter = new HtmlTagConverter
                {
                    HtmlOpenTag = "<h1>",
                    HtmlCloseTag = "</h1>",
                    TrimFromStartCount = 2,
                    TrimFromEndCount = 1
                },
                TagType = TagType.Header
            };
        }

        public static IEnumerable<IToken> GetTokens()
        {
            yield return Italic();
            yield return Bold();
            yield return Header();
        }
    }
}