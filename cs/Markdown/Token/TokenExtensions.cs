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
        var digitInLeft = openingToken.Position > 0 && char.IsDigit(source[openingToken.Position-1]);
        var digitInRight = token.Position + token.Length <= source.Length - 1 &&
                           char.IsDigit(source[token.Position + token.Length]);
        return digitInLeft && digitInRight;
    }

    private static bool IsInDifferentWords(this IToken token, IToken openingToken, string source)
    {
        var charInLeft = openingToken.Position > 0 && char.IsLetterOrDigit(source[openingToken.Position-1]);
        var charInRight = token.Position + token.Length <= source.Length - 1 &&
                          char.IsLetterOrDigit(source[token.Position + token.Length]);
        return charInLeft && charInRight && source.Substring(openingToken.Position, token.Position).Contains(' ');
    }

    public static bool IsValidOpen(this IToken token, string source)
    {
        return (source.Length - 1 >= token.Position + token.Length && source[token.Position + token.Length] != ' ');
    }
    
    public static bool IsValidClose(this IToken token, string source)
    {
        return source[token.Position - 1] != ' ';
    }
}