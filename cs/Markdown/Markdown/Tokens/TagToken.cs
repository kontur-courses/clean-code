using Markdown.Tags;

namespace Markdown.Tokens;

public class TagToken : IToken<Tag>
{
    public TagToken(int startIndex, int endIndex, Tag value)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Value = value;
    }

    public int StartIndex { get; }
    public int EndIndex { get; }
    public Tag Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }
}