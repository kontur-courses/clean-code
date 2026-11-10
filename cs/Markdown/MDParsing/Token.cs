namespace Markdown.MDParsing
{
    public struct Token
    {
        public string Text { get; init; }
        public int UnderscoreCount { get; init; }

        public Token(string text = "", int underscoreCount = 0)
        {
            Text = text;
            UnderscoreCount = underscoreCount;
        }
    }
}
