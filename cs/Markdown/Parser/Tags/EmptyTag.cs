using System;
using Markdown.Tree;

namespace Markdown.Parser.Tags
{
    internal class EmptyTag : MarkdownTag
    {
        public override string String => string.Empty;
        public override Node Node => new PlainTextNode(null);
        public override string Name => "empty";

        public static readonly EmptyTag Instance = new EmptyTag();
    }
}