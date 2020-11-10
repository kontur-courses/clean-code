namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    /// <summary>
    /// Однородная группа символов из текста
    /// Может быть просто текстом, либо каким-либо управляющим символом, например _ или __
    /// </summary>
    public abstract class Token
    {
        public Token(int startPosition, string rawText)
        {
            StartPosition = startPosition;
            RawText = rawText;
        }

        public int StartPosition { get; }

        public string RawText { get; }
    }
}