using Markdown.Tag;

namespace Markdown;

public class Token
{
    public Token(TagType type, int startIndex, int endIndex)
    {
        TagType = type;
        StartIndex = startIndex;
        EndIndex = endIndex;
    }

    public TagType TagType { get; }
    public int StartIndex { get; }
    public int EndIndex { get; }
}