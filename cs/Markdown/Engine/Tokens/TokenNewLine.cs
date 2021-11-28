namespace Markdown.Engine.Tokens
{
    public class TokenNewLine: IToken
    {
        public string Value => "\n";
        public TokenType TokenType => TokenType.NewLine;
        public bool CanParse(string symbol) => symbol == Value;
        public IToken Create(string[] text, int index) => new TokenNewLine();
    }
}