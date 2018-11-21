namespace Markdown.Data
{
    public enum TokenType
    {
        Text,
        Tag,
        Space,
        ParagraphStart,
        ParagraphEnd
    }

    public static class TokenTypeExtensions
    {
        public static bool IsSeparator(this TokenType tokenType) =>
            tokenType == TokenType.Space ||
            tokenType == TokenType.ParagraphStart || tokenType == TokenType.ParagraphEnd;
    }
}