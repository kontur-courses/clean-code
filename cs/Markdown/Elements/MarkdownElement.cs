using System.Collections.Generic;

namespace Markdown.Elements
{
    public class MarkdownElement : IMarkdownElement
    {
        public IElementType ElementType { get; protected set; }

        public string Markdown { get; protected set; }

        public int StartPosition { get; protected set; }

        public int EndPosition { get; protected set; }

        public IReadOnlyList<IMarkdownElement> InnerElements { get; protected set; }

        public MarkdownElement(IElementType type, string markdown, int start, int end,
            IReadOnlyList<IMarkdownElement> innerElements)
        {
            ElementType = type;
            Markdown = markdown;
            StartPosition = start;
            EndPosition = end;
            InnerElements = innerElements;
        }
    }
}
