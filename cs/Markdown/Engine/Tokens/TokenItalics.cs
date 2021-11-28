namespace Markdown.Engine.Tokens
{
    public class TokenItalics : IToken
    {
        public string Value => "_";
        public bool CanParse(string symbol) => symbol == Value;
        public IToken Create(string[] text, int index) => new TokenItalics();
        public TokenType TokenType => TokenType.Italics;
    }
}