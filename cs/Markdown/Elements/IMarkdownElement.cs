using System.Collections.Generic;

namespace Markdown.Elements
{
    public interface IMarkdownElement
    {
        IElementType ElementType { get; }
        string Markdown { get; }
        int StartPosition { get; }
        int EndPosition { get; }
        IReadOnlyList<IMarkdownElement> InnerElements { get; }
    }
}
