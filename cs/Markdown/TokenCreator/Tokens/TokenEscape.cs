namespace Markdown
{
    public class TokenEscape: IToken
    {
        public string Value => "\\";
        public TokenType TokenType => TokenType.Escape;
        public IToken Create(string[] text, int index) => new TokenEscape();
    }
}