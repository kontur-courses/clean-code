using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser;

public static class UnderscoreTagMatcher
{
    public static Dictionary<int, int> MatchTags(IReadOnlyList<Token> tokens, int startIndex, int endIndex, HashSet<int>? ignoredIndices = null)
    {
        var matchedPairs = new Dictionary<int, int>();
        var stack = new List<int>();
        var lastWhitespaceIndex = -1;

        for (var i = startIndex; i <= endIndex; i++)
        {
            if (ignoredIndices is not null && ignoredIndices.Contains(i))
                continue;

            var token = tokens[i];

            if (token.Type == TokenType.Whitespace)
            {
                lastWhitespaceIndex = i;
                continue;
            }

            if (token.Type is not (TokenType.SingleUnderscore or TokenType.DoubleUnderscore))
                continue;

            var matched = false;

            if (token.CanClose && stack.Count > 0)
            {
                var topIndex = stack[^1];
                var topToken = tokens[topIndex];

                if (topToken.Type == token.Type)
                {
                    stack.RemoveAt(stack.Count - 1);
                    if (IsValidPair(topToken, token, topIndex, i, lastWhitespaceIndex))
                    {
                        matchedPairs[topIndex] = i;
                        matched = true;
                    }
                }
                else if (stack.Count >= 2 && tokens[stack[^2]].Type == token.Type)
                {
                    var outerIndex = stack[^2];
                    if (IsValidPair(tokens[outerIndex], token, outerIndex, i, lastWhitespaceIndex))
                    {
                        stack.RemoveAt(stack.Count - 1);
                        stack.RemoveAt(stack.Count - 1);
                        continue;
                    }
                }
            }

            if (!matched && token.CanOpen)
            {
                stack.Add(i);
            }
        }

        DisableBoldInsideItalic(tokens, startIndex, endIndex, matchedPairs);
        return matchedPairs;
    }

    private static bool IsValidPair(
        Token openToken,
        Token closeToken,
        int openIndex,
        int closeIndex,
        int lastWhitespaceIndex)
    {
        if (closeIndex <= openIndex + 1)
            return false;

        var crossesWords = (openToken.IsInsideWord || closeToken.IsInsideWord)
                           && lastWhitespaceIndex > openIndex;

        return !crossesWords;
    }

    private static void DisableBoldInsideItalic(
        IReadOnlyList<Token> tokens,
        int startIndex,
        int endIndex,
        Dictionary<int, int> matchedPairs)
    {
        var closingToOpening = new Dictionary<int, int>();
        foreach (var (open, close) in matchedPairs)
            closingToOpening[close] = open;

        var italicDepth = 0;

        for (var i = startIndex; i <= endIndex; i++)
        {
            if (matchedPairs.TryGetValue(i, out _))
            {
                if (tokens[i].Type == TokenType.SingleUnderscore)
                    italicDepth++;
                else if (tokens[i].Type == TokenType.DoubleUnderscore && italicDepth > 0)
                    matchedPairs.Remove(i);
            }
            else if (closingToOpening.TryGetValue(i, out var openIdx))
            {
                if (tokens[openIdx].Type == TokenType.SingleUnderscore)
                    italicDepth--;
            }
        }
    }
}