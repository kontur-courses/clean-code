using System.Collections.Generic;
using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Impl.Italic
{
    public class MarkdownElementItalic : MarkdownElement
    {
        internal MarkdownElementItalic(int lastTokenIndex, ICollection<Token> content)
            : base(lastTokenIndex)
        {
            Content = content;
        }

        public ICollection<Token> Content { get; }
    }
}