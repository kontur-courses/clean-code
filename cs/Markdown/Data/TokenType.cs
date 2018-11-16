namespace Markdown.Data
{
    public enum TokenType
    {
        Text,
        Tag,
        Space,
        EscapeSymbol,
        NewLine,
        ParagraphStart,
        ParagraphEnd
    }

    public static class TokenTypeExtensions
    {
        public static bool IsSeparator(this TokenType tokenType) =>
            tokenType == TokenType.Space || tokenType == TokenType.NewLine ||
            tokenType == TokenType.ParagraphStart || tokenType == TokenType.ParagraphEnd;
    }
}