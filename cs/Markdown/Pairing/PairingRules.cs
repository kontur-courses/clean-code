using Markdown.Tokens;

namespace Markdown.Pairing;

public static class PairingRules
{
    public static bool IsDelimiter(this Token token)
    {
        return token.Kind is TokenKind.Underscore or TokenKind.DoubleUnderscore;
    }

    public static bool CanOpen(this Token token)
    {
        return token.IsDelimiter() && !token.HasSpaceAfter() && !token.IsNearDigit();
    }

    public static bool CanClose(this Token token)
    {
        return token.IsDelimiter() && !token.HasSpaceBefore() && !token.IsNearDigit();
    }

    public static bool HasSpaceBefore(this Token token)
    {
        return token.Before == Token.Boundary || char.IsWhiteSpace(token.Before);
    }

    public static bool HasSpaceAfter(this Token token)
    {
        return token.After == Token.Boundary || char.IsWhiteSpace(token.After);
    }

    public static bool IsInsideWord(this Token token)
    {
        return char.IsLetterOrDigit(token.Before) && char.IsLetterOrDigit(token.After);
    }

    public static bool IsNearDigit(this Token token)
    {
        return char.IsDigit(token.Before) || char.IsDigit(token.After);
    }
}