using System.Collections.Generic;
using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Models
{
    public class MarkdownDocument
    {
        private readonly List<MarkdownElement> elements = new List<MarkdownElement>();
        public IReadOnlyCollection<MarkdownElement> Elements => elements.AsReadOnly();

        public void Add(MarkdownElement element) => elements.Add(element);
        
        public static MarkdownDocument Empty => new MarkdownDocument();
    }
}