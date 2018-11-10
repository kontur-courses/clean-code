using System.Collections.Generic;

namespace Markdown
{
    interface IMarkdownElement
    {
        string Indicator { get; }
        int StartPosition { get; }
        int EndPosition { get; }
        IReadOnlyList<IMarkdownElement> InnerElements { get; }
        bool CanContainElement(IMarkdownElement element);
    }
}
