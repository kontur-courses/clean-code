namespace Markdown.Tags;

public class TagPosition
{
    public int Start { get; }
    public int End { get; }

    public TagPosition(int start, int end)
    {
        Start = start;
        End = end;
    }
}