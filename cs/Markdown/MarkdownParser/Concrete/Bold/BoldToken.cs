using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Bold
{
    public class BoldToken : PairedToken
    {
        public TokenPosition Position { get; }

        public BoldToken(int startPosition, string rawValue, TokenPosition position) 
            : base(startPosition, rawValue)
        {
            Position = position;
        }

        public static bool CanBeOpening(BoldToken token) => !token.Position.BeforeWhitespace();
        public static bool CanBeClosing(BoldToken token) => !token.Position.AfterWhitespace();
        
        public override TokenType Type { get; } = TokenType.Any;
    }
}