namespace Markdown;

public class Token
{
    public readonly ITag Tag;
    public readonly int Start;
    public readonly int End;

    public Token(ITag tag, int start, int end)
    {
        Tag = tag;
        Start = start;
        End = end;
    }
}