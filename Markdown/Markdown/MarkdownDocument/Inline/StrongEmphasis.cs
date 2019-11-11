using System.Collections.Generic;
using System.Linq;

namespace Markdown.MarkdownDocument.Inline
{
    public class StrongEmphasis : IInlineWithContent
    {
        public StrongEmphasis(IEnumerable<IInline> content)
        {
            Content = content;
        }

        public IEnumerable<IInline> Content { get; set; }
    }
}