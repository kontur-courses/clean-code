namespace Markdown.Markdown.Tags;

public interface ITagValidator
{
    bool IsValidTag(string content);
    bool IsTagStart(string content);
    bool IsTagEnd(string content);
}