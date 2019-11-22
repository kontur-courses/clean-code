using System;
using Markdown.Tree;

namespace Markdown.Parser.Tags
{
    internal class EmptyTag : MarkdownTag
    {
        public override string String => string.Empty;
        public override Node CreateNode() => new PlainTextNode(null);

        public static readonly EmptyTag Instance = new EmptyTag();
    }
}