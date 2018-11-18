using System.Collections.Generic;

namespace Markdown.Elements
{
    public class MarkdownElement
    {
        public IElementType ElementType { get; }

        public string Markdown { get; }

        public int StartPosition { get; }

        public int EndPosition { get; }

        public IReadOnlyList<MarkdownElement> InnerElements { get; }

        public MarkdownElement(IElementType type, string markdown, int start, int end,
            IReadOnlyList<MarkdownElement> innerElements)
        {
            ElementType = type;
            Markdown = markdown;
            StartPosition = start;
            EndPosition = end;
            InnerElements = innerElements;
        }
    }
}
