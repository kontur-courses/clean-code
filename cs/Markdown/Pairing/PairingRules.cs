using Markdown.Tokens;

namespace Markdown.Pairing;

public static class PairingRules
{
    public static bool CanOpen(this Token token)
    {
        return token.IsDelimiter && !token.HasSpaceAfter() && !token.IsNearDigit();
    }

    public static bool CanClose(this Token token)
    {
        return token.IsDelimiter && !token.HasSpaceBefore() && !token.IsNearDigit();
    }

    public static bool HasSpaceBefore(this Token token)
    {
        return IsBoundaryOrWhiteSpace(token.Before);
    }

    public static bool HasSpaceAfter(this Token token)
    {
        return IsBoundaryOrWhiteSpace(token.After);
    }

    public static bool IsInsideWord(this Token token)
    {
        return char.IsLetterOrDigit(token.Before) && char.IsLetterOrDigit(token.After);
    }

    public static bool IsNearDigit(this Token token)
    {
        return char.IsDigit(token.Before) || char.IsDigit(token.After);
    }

    private static bool IsBoundaryOrWhiteSpace(char symbol)
    {
        return symbol == Token.Boundary || char.IsWhiteSpace(symbol);
    }
}