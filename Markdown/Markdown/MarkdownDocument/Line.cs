using System.Collections.Generic;
using System.Linq;
using Markdown.MarkdownDocument.Inline;

namespace Markdown.MarkdownDocument
{
    public class Line
    {
        public IEnumerable<IInline> Elements { get; }

        public Line(IEnumerable<IInline> elements)
        {
            Elements = elements;
        }
    }
}