namespace Markdown.Data.TagsInfo
{
    public class MarkdownHeadingInfo : ITagInfo
    {
        public string OpeningTag { get; }
        public string ClosingTag => "\n";
        public bool CanBeInsideOtherTag => false;

        public bool MustBeOpened(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
              token == OpeningTag && previousTokenType.IsSeparator() && !isOpened;

        public bool MustBeClosed(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            token == ClosingTag && previousTokenType != TokenType.EscapeSymbol && isOpened;

        public MarkdownHeadingInfo(string openingTag)
        {
            OpeningTag = openingTag + " ";
        }
    }
}