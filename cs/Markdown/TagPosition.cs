namespace Markdown;

public class TagPosition
{
    public readonly int Start;
    public readonly int End;
    public readonly TagType Tag;

    public TagPosition(TagType tag, int start, int end)
    {
        Tag = tag;
        End = end;
        Start = start;
    }
}