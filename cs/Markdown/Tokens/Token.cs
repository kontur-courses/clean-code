using Markdown.Tokens.Types;

namespace Markdown.Tokens;

public class Token
{
    public ITokenType TokenType { get; }

    public bool IsClosingTag { get; }

    public int StartingIndex { get; }

    public int Length { get; }

    public Token(ITokenType tokenType, bool isClosingTag, int startingIndex, int length)
    {
        TokenType = tokenType;
        IsClosingTag = isClosingTag;
        StartingIndex = startingIndex;
        Length = length;
    }

    public string GetValue()
    {
        return TokenType.Representation(IsClosingTag);
    }
}