using System.Collections.Generic;
using System.Linq;

namespace Markdown.MarkdownDocument.Inline
{
    public class Emphasis : IInlineWithContent
    {
        public Emphasis(IEnumerable<IInline> content)
        {
            Content = content;
        }

        public IEnumerable<IInline> Content { get; set; }
    }
}