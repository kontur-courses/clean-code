using Markdown.Tags;

namespace Markdown.Tokens;

public class TagToken: IToken<Tag>
{
    public int StartIndex { get; }
    public int EndIndex { get; }
    public Tag Value { get; }

    public TagToken(int startIndex, int endIndex, Tag value)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Value = value;
    }
}