using System;

namespace Markdown.Parser.Tags
{
    internal class EmptyTag : MarkdownTag
    {
        public override string String => string.Empty;
        public override TokenType TokenTypeStart => throw new InvalidOperationException();
        public override TokenType TokenTypeEnd => throw new InvalidOperationException();

        public static readonly EmptyTag Instance = new EmptyTag();
    }
}