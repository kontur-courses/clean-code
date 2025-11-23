namespace Markdown;

public static class ScoreUtils
{
    public static bool IsWhiteSpaceOrEndToken(Token? token) =>
        token is { Type: TokenType.WhiteSpace } or { Type: TokenType.EndOfText };
    
    public static bool TokenBetweenDigits(Token? previousToken, Token? nextToken)
    {
        var leftTokenIsDigit = previousToken is { Type: TokenType.Text, Value.Length: > 0 }
                               && char.IsDigit(previousToken.Value[^1]);
        var rightTokenIsDigit = nextToken is { Type: TokenType.Text, Value.Length: > 0 }
                                && char.IsDigit(nextToken.Value[0]);
        return leftTokenIsDigit && rightTokenIsDigit;
    }
    
}
