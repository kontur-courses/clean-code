namespace Markdown
{
    public class TokenStrong : IToken
    {
        public string Value => "__";
        public IToken Create(string[] text, int index) => new TokenStrong();
        public TokenType TokenType => TokenType.Strong;
    }
}