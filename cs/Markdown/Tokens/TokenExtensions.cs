using Markdown.Tokens;

namespace Markdown.Extensions;

public static class TokenExtensions
{
    public static bool IsSeparatorsInsideDifferentWords(this Token token, string str)
    {
        var isOpenInsideWord = token.OpeningIndex > 0 && !char.IsWhiteSpace(str[token.OpeningIndex - 1]);
        var isCloseInsideWord = token.ClosingIndex < str.Length - 1 && !char.IsWhiteSpace(str[token.ClosingIndex + 1]);

        return (isOpenInsideWord || isCloseInsideWord) &&
               (str.Substring(token.OpeningIndex, token.ClosingIndex - token.OpeningIndex)).Contains(" ");
    }

    public static bool IsTokenHasNoContent(this Token token)
    {
        return token.ClosingIndex - token.OpeningIndex <= token.Separator.Length * 2 - 1;
    }

    public static IEnumerable<Token> ReplaceInvalidTokenToLiteral(this Token token)
    {
        yield return new LiteralToken(token.OpeningIndex, token.OpeningIndex + token.Separator.Length - 1,
            token.Separator);

        if (!token.IsSingleSeparator)
        {
            yield return new LiteralToken(token.ClosingIndex - token.Separator.Length + 1, token.ClosingIndex,
                token.Separator);
        }
    }
}