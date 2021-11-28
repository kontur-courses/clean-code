namespace Markdown.Engine.Tokens
{
    public class SquareBracketClose : IToken
    {
        public TokenType TokenType => TokenType.SquareBracketClose;
        public string Value => "]";
        public bool CanParse(string symbol) => symbol == Value;
        public IToken Create(string[] text, int index) => new SquareBracketClose();
    }
}