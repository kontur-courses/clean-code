namespace Markdown.Tokens.CommonTokens;

public abstract class CommonToken : IToken
{
    public abstract TokenType Type { get; }
    public abstract string Content { get; }

    public static TokenType GetCommonTokenType(char symbol)
    {
        if (char.IsDigit(symbol))
            return TokenType.Digit;
        if (char.IsWhiteSpace(symbol))
            return TokenType.Space;
        if (symbol == '\\')
            return TokenType.Escape;

        return TokenType.Text;
    }
    
    public static CommonToken CreateCommonToken(TokenType tokenType, string content)
    {
        return tokenType switch
        {
            TokenType.Digit => new DigitToken(content),
            TokenType.Space => new SpaceToken(),
            TokenType.Escape => new EscapeToken(),
            _ => new TextToken(content)
        };
    }
}