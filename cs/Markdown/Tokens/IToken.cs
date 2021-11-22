using Markdown.Tags;

namespace Markdown.Tokens
{
    public interface IToken
    {
        Tag TagType { get; }
        int StartPosition { get; }
        int EndPosition { get; }
        string Value { get; }

        bool IsNestedInToken(IToken otherToken);
    }
}