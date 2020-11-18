using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Italic
{
    public class MarkdownElementItalic : MarkdownElement
    {
        public MarkdownElementItalic(Token[] tokens) : base(tokens)
        {
            Content = tokens.Skip(1).SkipLast(1).ToArray();
        }

        public MarkdownElementItalic(PairedToken startToken, ICollection<Token> content, PairedToken endToken)
            : base(new Token[] {new TokenPair(startToken, content.ToArray(), endToken)})
        {
            Content = content;
        }

        public ICollection<Token> Content { get; }
    }
}