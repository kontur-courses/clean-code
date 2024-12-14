namespace Markdown.Tags;

public class DefaultTagHandler : ITagHandler
{
    public bool IsTagStart(string text, int index)
    {
        return true;
    }

    public int ProcessTag(ref string text, int startIndex)
    {
        return startIndex + 1;
    }
}
