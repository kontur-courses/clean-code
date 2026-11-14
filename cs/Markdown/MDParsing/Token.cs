namespace Markdown.MDParsing
{
    // Посчитал не нужным выделят в отдельный файл, так как это используется только для этой структуры
    public enum TokenType
    {
        Text,
        EscapedSymbol,
        Underscores
    }

    public struct Token
    {
        private readonly string? _text;

        public TokenType Type { get; }

        public string Text => _text ?? string.Empty;

        public int UnderscoreCount { get; }

        public bool IsEscaped => Type == TokenType.EscapedSymbol;
        public bool IsUnderscores => Type == TokenType.Underscores;

        private Token(TokenType type, string? text, int underscoreCount)
        {
            Type = type;
            _text = text;
            UnderscoreCount = underscoreCount;
        }

        public static Token CreateText(string text) =>
            new(TokenType.Text, text ?? throw new ArgumentNullException(nameof(text)), 0);

        public static Token CreateEscaped(char symbol) =>
            new(TokenType.EscapedSymbol, symbol.ToString(), 0);

        public static Token CreateUnderscores(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            return new(TokenType.Underscores, null, count);
        }
    }
}