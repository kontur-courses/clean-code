namespace Markdown.Engine.Tokens
{
    public class TokenHeader1 : IToken
    {
        public string Value => "# ";
        public TokenType TokenType => TokenType.Header1;
        public bool CanParse(string symbol) => symbol == Value;
        public IToken Create(string[] text, int index) => new TokenHeader1();
    }
}