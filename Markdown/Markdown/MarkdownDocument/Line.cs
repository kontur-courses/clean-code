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

        public bool Equals(Line other)
        {
            if (Elements.Count() != other.Elements.Count())
                return false;
            var elements = Elements.ToArray();
            var otherElements = other.Elements.ToArray();

            return !elements.Where((t, i) => t.Equals(otherElements[i])).Any();
        }
    }
}