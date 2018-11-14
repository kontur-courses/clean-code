namespace Markdown.Data.TagsInfo
{
    public class BoldTagInfo : ITagInfo
    {
        public string OpeningTag => "__";
        public string ClosingTag => "__";
        public bool CanBeInsideOtherTag => false;

        public bool MustBeOpened(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType != TokenType.EscapeSymbol && previousTokenType == TokenType.Space &&
            nexTokenType != TokenType.Space && !isOpened;

        public bool MustBeClosed(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType != TokenType.EscapeSymbol && previousTokenType != TokenType.Space &&
            nexTokenType == TokenType.Space && isOpened;
    }
}