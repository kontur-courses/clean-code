using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Models
{
    public class MarkdownDocument
    {
        private readonly List<MarkdownElement> elements;

        public MarkdownDocument(IEnumerable<MarkdownElement> elements)
        {
            this.elements = elements.ToList();
        }

        public IReadOnlyCollection<MarkdownElement> Elements => elements.AsReadOnly();

        public void Add(MarkdownElement element) => elements.Add(element);

        public static MarkdownDocument Empty => new MarkdownDocument(new List<MarkdownElement>());
    }
}