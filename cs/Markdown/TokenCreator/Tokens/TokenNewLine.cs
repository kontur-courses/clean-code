namespace Markdown
{
    public class TokenNewLine: IToken
    {
        public string Value => "\n";
        public TokenType TokenType => TokenType.NewLine;
        public IToken Create(string[] text, int index) => new TokenNewLine();
    }
}