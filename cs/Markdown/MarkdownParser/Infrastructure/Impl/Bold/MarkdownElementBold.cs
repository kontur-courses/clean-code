using System.Collections.Generic;
using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Impl.Bold
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