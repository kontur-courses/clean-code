using Markdown.Tags;

namespace Markdown.Tokens
{
    public interface IToken
    {
        Tag Tag { get; }
        int StartPosition { get; }
        int EndPosition { get; }
        string Value { get; }

        bool IsNestedInAnotherToken(IToken anotherToken);
    }
}