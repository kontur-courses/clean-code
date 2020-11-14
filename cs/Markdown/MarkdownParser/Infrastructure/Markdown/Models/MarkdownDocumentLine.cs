using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Markdown.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Models
{
    public class MarkdownDocumentLine
    {
        private readonly List<MarkdownElement> elements;

        public MarkdownDocumentLine(IEnumerable<MarkdownElement> elements)
        {
            this.elements = elements.ToList();
        }

        public IReadOnlyCollection<MarkdownElement> Elements => elements.AsReadOnly();

        public void Add(MarkdownElement element) => elements.Add(element);

        public static MarkdownDocumentLine Empty => new MarkdownDocumentLine(new List<MarkdownElement>());
    }
}