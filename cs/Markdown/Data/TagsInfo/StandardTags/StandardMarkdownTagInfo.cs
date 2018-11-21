namespace Markdown.Data.TagsInfo.StandardTags
{
    public class StandardMarkdownTagInfo : ITagInfo
    {
        public string OpeningTag { get; }
        public string ClosingTag { get; }
        public bool CanBeInsideOtherTag { get; }

        public bool MustBeOpened(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            previousTokenType.IsSeparator() && !nexTokenType.IsSeparator() && !isOpened;

        public bool MustBeClosed(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType) =>
            !previousTokenType.IsSeparator() && nexTokenType.IsSeparator() && isOpened;

        public StandardMarkdownTagInfo(string tag, bool canBeInsideOtherTag)
        {
            OpeningTag = tag;
            ClosingTag = tag;
            CanBeInsideOtherTag = canBeInsideOtherTag;
        }
    }
}