namespace Markdown
{
    public class TokenHeader1 : IToken
    {
        public string Value => "# ";
        public TokenType TokenType => TokenType.Header1; 
        public IToken Create(string[] text, int index) => new TokenHeader1();
    }
}