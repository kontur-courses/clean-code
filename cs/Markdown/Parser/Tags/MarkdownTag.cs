using System;
using Markdown.Parser.TagsParsing;
using Markdown.Tree;

namespace Markdown.Parser.Tags
{
    public abstract class MarkdownTag
    {
        public abstract string String { get; }

        public abstract Node Node { get; }
    }
}