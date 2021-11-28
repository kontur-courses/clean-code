namespace Markdown.TokenCreator.Tokens
{
    public class TokenLink : IToken
    {
        public TokenType TokenType => TokenType.Link;
        public string Value { get; private init;}

        public bool CanParse(string symbol) => false;

        public IToken Create(string[] text, int index)
        {
            return new TokenLink{Value = string.Join("", text)};
        }
    }
}