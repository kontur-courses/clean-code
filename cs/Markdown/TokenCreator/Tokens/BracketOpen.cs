namespace Markdown.TokenCreator.Tokens
{
    public class BracketOpen : IToken
    {
        public TokenType TokenType => TokenType.BracketOpen;
        public string Value => "(";
        public bool CanParse(string symbol) => symbol == Value;

        public IToken Create(string[] text, int index) => new BracketOpen();
    }
}