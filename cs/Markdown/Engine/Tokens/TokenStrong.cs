namespace Markdown.Engine.Tokens
{
    public class TokenStrong : IToken
    {
        public string Value => "__";
        public bool CanParse(string symbol) => symbol == Value;
        public IToken Create(string[] text, int index) => new TokenStrong();
        public TokenType TokenType => TokenType.Strong;
    }
}