using System.Collections.Generic;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Bold
{
    public class MarkdownElementBold : MarkdownElement
    {
        public ICollection<MarkdownElement> Content { get; }

        public MarkdownElementBold(BoldToken opening, MarkdownElement[] content, Token[] tokens, BoldToken closing)
            : base(new Token[] {new TokenPair(opening, tokens, closing)})
        {
            Content = content;
        }
    }
}