using System.Collections.Generic;

namespace Markdown.MarkdownDocument.Inline
{
    public interface IInlineWithContent : IInline
    {
        IEnumerable<IInline> Content { get; set; }
    }
}