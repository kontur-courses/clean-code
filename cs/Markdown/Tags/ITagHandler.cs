namespace Markdown.Tags;
public interface ITagHandler
{
    bool IsTagStart(string text, int index);

    int ProcessTag(ref string text, int startIndex);
}
