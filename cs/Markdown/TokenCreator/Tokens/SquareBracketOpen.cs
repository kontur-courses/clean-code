namespace Markdown.TokenCreator.Tokens
{
    public class SquareBracketOpen : IToken
    {
        public TokenType TokenType => TokenType.SquareBracketOpen;
        public string Value => "[";
        public bool CanParse(string symbol) => symbol == Value;

        public IToken Create(string[] text, int index) => new SquareBracketOpen();
    }
}