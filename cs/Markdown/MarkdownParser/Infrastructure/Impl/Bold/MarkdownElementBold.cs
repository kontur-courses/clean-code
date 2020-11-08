using System.Collections.Generic;
using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Impl.Bold
{
    public class MarkdownElementBold : MarkdownElement
    {
        public ICollection<MarkdownElement> Content { get; }

        public MarkdownElementBold(int lastTokenIndex, ICollection<MarkdownElement> content) : base(lastTokenIndex)
        {
            Content = content;
        }
    }
}