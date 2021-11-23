namespace Markdown.Tokens
{
    public interface IToken
    {
        public TokenType Type { get; }

        public string Value { get; }
    }
}