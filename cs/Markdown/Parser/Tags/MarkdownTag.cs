using System;
using Markdown.Parser.TagsParsing;

namespace Markdown.Parser.Tags
{
    public abstract class MarkdownTag
    {
        public abstract string String { get; }
        public abstract TokenType TokenTypeStart { get; }
        public abstract TokenType TokenTypeEnd { get; }

        public TokenType GetTokenTypeByEventType(TagEventType type)
        {
            switch (type)
            {
                case TagEventType.Start:
                    return TokenTypeStart;
                case TagEventType.End:
                    return TokenTypeEnd;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}