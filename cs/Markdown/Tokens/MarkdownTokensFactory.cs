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
                TagConverter = new TagConverter()
                {
                    OpenTag = "<em>",
                    CloseTag = "</em>",
                    GetTrimFromEndCount = 1,
                    GetTrimFromStartCount = 1
                },
                TagType = TagType.Italic
            };
        }

        public static IToken Bold()
        {
            return new Token
            {
                Pattern = new PairedTokenPattern("__"),
                TagConverter = new TagConverter()
                {
                    OpenTag = "<strong>",
                    CloseTag = "</strong>",
                    GetTrimFromEndCount = 2,
                    GetTrimFromStartCount = 2
                },
                TagType = TagType.Bold
            };
        }

        public static IEnumerable<IToken> GetTokens()
        {
            yield return Italic();
            yield return Bold();
        }
    }
}