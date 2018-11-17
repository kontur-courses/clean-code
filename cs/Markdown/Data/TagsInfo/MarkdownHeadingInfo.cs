namespace Markdown.Data.TagsInfo
{
    public class MarkdownHeadingInfo : ITagInfo
    {
        public string OpeningTag { get; }
        public string ClosingTag => "\n";
        public bool CanBeInsideOtherTag => false;

        public bool MustBeOpened(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType.IsSeparator() && nexTokenType == TokenType.Space && !isOpened;

        public bool MustBeClosed(bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType != TokenType.EscapeSymbol && isOpened;

        public MarkdownHeadingInfo(string openingTag)
        {
            OpeningTag = openingTag;
        }
    }
}