namespace Markdown.Data.TagsInfo.Headings
{
    public class MarkdownHeadingInfo : ITagInfo
    {
        public string OpeningTag { get; }
        public string ClosingTag => "\n";
        public bool CanBeInsideOtherTag => false;

        public bool MustBeOpened(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
              token == OpeningTag && previousTokenType.IsSeparator() && !isOpened;

        public bool MustBeClosed(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            token == ClosingTag && isOpened;

        public MarkdownHeadingInfo(int numberOfSharps)
        {
            OpeningTag = new string('#', numberOfSharps) + " ";
        }
    }
}