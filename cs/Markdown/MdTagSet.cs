using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public static class MdTagSet
    {
        public static readonly IReadOnlyDictionary<string, Func<int, IToken>> TokensByTags = new Dictionary<string, Func<int, IToken>>
        {
            { ItalicToken.MdTag, position => new ItalicToken(position) },
            { BoldToken.MdTag, position => new BoldToken(position) },
            { HeadingToken.MdTag, position => new HeadingToken(position, true) },
            { EscapeToken.MdTag, position => new EscapeToken(position) },
            { HeadingToken.NewParagraphSymbols[0], position => new HeadingToken(position, false)},
            { HeadingToken.NewParagraphSymbols[1], position => new HeadingToken(position, false)}
        };
    }
}
