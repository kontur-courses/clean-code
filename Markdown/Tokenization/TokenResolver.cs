using Markdown.Tokenization.Tokens;
using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization;

public static class TokenResolver
{
    public static List<Token> Resolve(IEnumerable<Token> unresolvedTokens)
    {
        var tokens = unresolvedTokens.ToList();
        var lastWordBreaks = FindLastWordBreaks(tokens);

        return tokens;
    }

    private static bool IsValidPair(
        PairToken opening, PairToken closing, int lastWordBreak)
    {
        if (opening.Start + opening.Length == closing.Start)
            return false;

        if (opening.IsInsideWord || closing.IsInsideWord)
            return lastWordBreak < opening.Start + opening.Length;

        return true;
    }

    private static int[] FindLastWordBreaks(List<Token> tokens)
    {
        var breaks = new int[tokens.Count];
        var (current, lastBreak) = (-1, -1);

        for (var tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
        {
            if (tokens[tokenIndex] is not PairToken pairToken)
                continue;

            if (current == -1)
                current = pairToken.Start;

            while (current < pairToken.Start)
            {
                var symbol = pairToken.Source[current];

                if (!char.IsLetterOrDigit(symbol) && symbol != '_')
                    lastBreak = current;

                current++;
            }

            breaks[tokenIndex] = lastBreak;
        }

        return breaks;
    }
}
