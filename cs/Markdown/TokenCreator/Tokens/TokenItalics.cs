namespace Markdown
{
    public class TokenItalics : IToken
    {
        public string Value => "_";
        public IToken Create(string[] text, int index) => new TokenItalics();
        public TokenType TokenType => TokenType.Italics;
    }
}