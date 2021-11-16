using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Models
{
    public static class MarkdownTokensFactory
    {
        public static IToken Italic()
        {
            return new Token()
            {
                Pattern = new ItalicPattern(),
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