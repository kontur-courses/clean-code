namespace Markdown;

public static class StackExtensions
{
    public static bool IsPeekEqual(this Stack<OpenToken> tokensWithOpenTag, TagType tagType)
    {
        return tokensWithOpenTag.Count > 0 && tokensWithOpenTag.Peek().OpenTag.TagType == tagType;
    }
}
