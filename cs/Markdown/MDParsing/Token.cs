namespace Markdown.MDParsing
{
    public struct Token
    {
        public string Text { get; init; }
        public int UnderscoreCount { get; init; }

        public bool IsEscaped { get; init; }

        public Token(string text = "", int underscoreCount = 0, bool isEscaped = false)
        {
            Text = text;
            UnderscoreCount = underscoreCount;
            IsEscaped = isEscaped;
        }
    }
}
