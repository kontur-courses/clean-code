namespace Markdown
{
    internal enum TokenType
    {
        Text,
        Space,
        Newline,
        Sharp,
        Object
    }

    internal class Token
    {
        public virtual TokenType Type { get; set; }
        public string Text { get; set; }

        public static Token EmptyText => new Token() { Type = TokenType.Text, Text = string.Empty };

        public static Token CreateTextToken(string text) => new Token() { Type = TokenType.Text, Text = text };
    }
}