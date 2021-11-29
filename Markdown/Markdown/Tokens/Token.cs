namespace Markdown.Tokens
{
    // Решил переименовать токены из MarkdownToken в просто Token,
    // т.к. больше они никак не зависят от того, к какой они принадлежат разметке
    public class Token : IToken
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}