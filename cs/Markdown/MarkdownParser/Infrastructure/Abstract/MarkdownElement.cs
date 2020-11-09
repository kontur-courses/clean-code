namespace MarkdownParser.Infrastructure.Abstract
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