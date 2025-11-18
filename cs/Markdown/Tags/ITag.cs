namespace Markdown.Tags;

public interface ITag
{
    string MdTag { get; }
    string HtmlOpenTag { get; }
    string HtmlCloseTag { get; }
    bool CanBeOpened(char left, char right);
    bool CanBeClosed(char left, char right);
}