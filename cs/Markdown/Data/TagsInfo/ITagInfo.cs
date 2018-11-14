namespace Markdown.Data.TagsInfo
{
    public interface ITagInfo
    {
        string OpeningTag { get; }
        string ClosingTag { get; }
        bool CanBeInsideOtherTag { get; }

        bool MustBeOpened(bool isOpened, TokenType previousTokenType, TokenType nexTokenType);
        bool MustBeClosed(bool isOpened, TokenType previousTokenType, TokenType nexTokenType);
    }
}