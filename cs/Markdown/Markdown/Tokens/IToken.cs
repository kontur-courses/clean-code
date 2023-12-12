namespace Markdown.Tokens;

public interface IToken
{
    string Value { get; }
    int StartIndex { get; }
    int EndIndex { get; }
}