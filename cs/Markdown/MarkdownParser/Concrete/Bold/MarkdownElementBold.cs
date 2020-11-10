using System.Collections.Generic;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Bold
{
    public class MarkdownElementBold : MarkdownElement
    {
        public ICollection<MarkdownElement> Content { get; }

        public MarkdownElementBold(ICollection<MarkdownElement> content, Token[] tokens) : base(tokens)
        {
            Content = content;
        }
    }
}