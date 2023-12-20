namespace Markdown.Token;

public static class TokenExtensions
{
    public static bool IsPairedTokenValidPositioned(this IToken token, IToken openingToken, string source)
    {
        return !(token.IsEmpty(openingToken) || token.IsBetweenDigits(openingToken, source) ||
                 token.IsInDifferentWords(openingToken, source));
    }

    private static bool IsEmpty(this IToken token, IToken openingToken)
    {
        return token.Position - (openingToken.Position + openingToken.Length) == 0;
    }

    private static bool IsBetweenDigits(this IToken token, IToken openingToken, string source)
    {
        var digitInLeft = token.Position > 0 && char.IsDigit(source[token.Position]);
        var digitInRight = token.Position + token.Length < source.Length - 1 &&
                           char.IsDigit(source[token.Position + 1]);
        return digitInLeft && digitInRight;
    }

    private static bool IsInDifferentWords(this IToken token, IToken openingToken, string source)
    {
        var charInLeft = token.Position > 0 && char.IsLetterOrDigit(source[token.Position]);
        var charInRight = token.Position + token.Length < source.Length - 1 &&
                          char.IsLetterOrDigit(source[token.Position + 1]);
        return charInLeft && charInRight && source.Substring(openingToken.Position, token.Position).Contains(' ');
    }
}