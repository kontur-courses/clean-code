namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    /// <summary>
    /// Однородная группа символов из текста
    /// Может быть просто текстом, либо каким-либо управляющим символом, например _ или __
    /// </summary>
    public abstract class Token
    {
        public Token(int startPosition, int rawLength, string rawText)
        {
            StartPosition = startPosition;
            RawLength = rawLength;
            RawText = rawText;
        }

        public int StartPosition { get; }

        /// <summary>
        /// Сколько символов занимает в исходном тексте
        /// </summary>
        public int RawLength { get; }

        public string RawText { get; }
    }
}