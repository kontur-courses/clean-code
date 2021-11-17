using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens
{
    public static class MarkdownTokensFactory
    {
        public static IToken Italic()
        {
            return new Token()
            {
                Pattern = new PairedTokenPattern("_"),
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

        public static IEnumerable<IToken> GetTokens()
        {
            yield return Italic();
        }
    }
}