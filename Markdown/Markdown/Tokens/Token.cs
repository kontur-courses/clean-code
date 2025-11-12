using Markdown.Tokens.Tags;

namespace Markdown.Tokens;

public class Token(ITag tag, string value, int endIndex)
{
    public ITag Tag { get; } = tag;
    public string Value { get; } = value;
    public int EndIndex { get; } = endIndex;
}