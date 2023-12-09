using Markdown.Tokens.Types;

namespace Markdown.Tokens;

public class Token
{
    public ITokenType TokenType { get; }

    private readonly bool isClosingTag;

    public Token(ITokenType tokenType, bool isClosingTag)
    {
        TokenType = tokenType;
        this.isClosingTag = isClosingTag;
    }

    public string GetValue()
    {
        return TokenType.Representation(isClosingTag);
    }
}