using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    public abstract class MarkdownElement
    {
        protected MarkdownElement(Token[] tokens)
        {
            Tokens = tokens;
        }

        public Token[] Tokens { get; }
    }
}