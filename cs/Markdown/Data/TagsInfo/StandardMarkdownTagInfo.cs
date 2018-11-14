namespace Markdown.Data.TagsInfo
{
    public class StandardMarkdownTagInfo : ITagInfo
    {
        public string OpeningTag { get; }
        public string ClosingTag { get; }
        public bool CanBeInsideOtherTag { get; }

        public bool MustBeOpened(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType != TokenType.EscapeSymbol && previousTokenType == TokenType.Space &&
            nexTokenType != TokenType.Space && !isOpened;

        public bool MustBeClosed(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType != TokenType.EscapeSymbol && previousTokenType != TokenType.Space &&
            nexTokenType == TokenType.Space && isOpened;

        public StandardMarkdownTagInfo(string tag, bool canBeInsideOtherTag)
        {
            OpeningTag = tag;
            ClosingTag = tag;
            CanBeInsideOtherTag = canBeInsideOtherTag;
        }
    }
}