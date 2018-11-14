namespace Markdown.Data.TagsInfo
{
    public class ItalicTagInfo : ITagInfo
    {
        public string OpeningTag => "_";
        public string ClosingTag => "_";
        public bool CanBeInsideOtherTag => true;

        public bool MustBeOpened(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType != TokenType.EscapeSymbol && previousTokenType == TokenType.Space &&
            nexTokenType != TokenType.Space && !isOpened;

        public bool MustBeClosed(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType != TokenType.EscapeSymbol && previousTokenType != TokenType.Space &&
            nexTokenType == TokenType.Space && isOpened;
    }
}