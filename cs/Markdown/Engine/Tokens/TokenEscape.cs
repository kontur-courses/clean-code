namespace Markdown.Engine.Tokens
{
    public class TokenEscape: IToken
    {
        public string Value => "\\";
        public TokenType TokenType => TokenType.Escape;
        public bool CanParse(string symbol) => symbol == Value;
        public IToken Create(string[] text, int index) => new TokenEscape();
    }
}