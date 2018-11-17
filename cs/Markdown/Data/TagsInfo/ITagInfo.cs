namespace Markdown.Data.TagsInfo
{
    public interface ITagInfo
    {
        string OpeningTag { get; }
        string ClosingTag { get; }
        bool CanBeInsideOtherTag { get; }

        bool MustBeOpened(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType);
        bool MustBeClosed(string token, bool isOpened, TokenType previousTokenType, TokenType nexTokenType);
    }
}