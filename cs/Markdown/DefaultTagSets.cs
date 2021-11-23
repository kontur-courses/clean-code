using System;
using System.Collections.Generic;
using Markdown.Renderer;
using Markdown.Tokens;

namespace Markdown
{
    public static class DefaultTagSets
    {
        public static readonly IReadOnlyDictionary<string, HtmlTag> HtmlTagsBySeparator = new Dictionary<string, HtmlTag>
        {
            { BoldToken.Separator, new HtmlTag("<strong>", "</strong>", true) },
            { ItalicToken.Separator, new HtmlTag("<em>", "</em>", true) },
            { HeaderToken.Separator, new HtmlTag("<h1>", "</h1>", true) },
            { ScreeningToken.Separator, new HtmlTag(string.Empty, string.Empty, false) },
            { ImageToken.Separator, new HtmlTag("<img >", string.Empty, false) }
        };

        public static readonly IReadOnlyDictionary<string, Func<int, Token>> TokensBySeparator = new Dictionary<string, Func<int, Token>>
        {
            { ItalicToken.Separator, index => new ItalicToken(index) },
            { BoldToken.Separator, index => new BoldToken(index) },
            { HeaderToken.Separator, index => new HeaderToken(index) },
            { ScreeningToken.Separator, index => new ScreeningToken(index) },
            { ImageToken.Separator, index => new ImageToken(index) }
        };
    }
}
