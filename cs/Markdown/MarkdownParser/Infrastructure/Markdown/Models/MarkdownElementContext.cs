using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Models
{
    public interface IMarkdownElementContext
    {
        Token[] NextTokens { get; }
        Token CurrentToken { get; }
    }

    public class MarkdownElementContext<TToken> : IMarkdownElementContext where TToken : Token
    {
        public MarkdownElementContext(TToken currentToken, Token[] nextTokens)
        {
            CurrentToken = currentToken;
            NextTokens = nextTokens;
        }

        public Token[] NextTokens { get; }
        public TToken CurrentToken { get; }
        Token IMarkdownElementContext.CurrentToken => CurrentToken;
    }
}