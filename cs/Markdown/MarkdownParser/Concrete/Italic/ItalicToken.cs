using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Italic
{
    public class ItalicToken : Token
    {
        public TokenPosition Position { get; }

        public ItalicToken(int startPosition, string rawValue, TokenPosition position)
            : base(startPosition, rawValue)
        {
            Position = position;
        }

        public static bool CanBeOpening(ItalicToken token) => !token.Position.BeforeWhitespace(); 
        public static bool CanBeClosing(ItalicToken token) => !token.Position.AfterWhitespace();
    }
}