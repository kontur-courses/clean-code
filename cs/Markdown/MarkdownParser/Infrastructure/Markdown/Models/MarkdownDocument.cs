using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Markdown.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Models
{
    public class MarkdownDocument
    {
        private readonly List<MarkdownDocumentLine> lines;

        public MarkdownDocument(IEnumerable<MarkdownDocumentLine> elements)
        {
            this.lines = elements.ToList();
        }

        public IReadOnlyCollection<MarkdownDocumentLine> Lines => lines.AsReadOnly();

        public void Add(MarkdownDocumentLine line) => lines.Add(line);

        public static MarkdownDocument Empty => new MarkdownDocument(new List<MarkdownDocumentLine>());
        public IEnumerable<MarkdownElement> AllElements => lines.SelectMany(l => l.Elements);
    }
}