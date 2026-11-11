using Markdown.Tokens;

namespace Markdown.Pairing;

public class TagPairer
{
    public void Pair(IReadOnlyList<Token> tokens)
    {
        var whitespacePrefix = BuildWhitespacePrefixSums(tokens);

        PairTokens(tokens, whitespacePrefix);

        if (DeactivateCrossingDelimiters(tokens))
        {
            ResetLinks(tokens);
            PairLinkBrackets(tokens);
        }

        DeactivateStrongInsideEm(tokens);
    }

    private static void PairTokens(IReadOnlyList<Token> tokens, int[] whitespacePrefix)
    {
        var delimiterOpeners = new Dictionary<TokenKind, Stack<int>>
        {
            [TokenKind.Underscore] = new(),
            [TokenKind.DoubleUnderscore] = new()
        };
        var linkOpeners = new Stack<LinkOpener>();

        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];

            if (token.IsDelimiter)
            {
                ProcessDelimiter(tokens, index, delimiterOpeners[token.Kind], linkOpeners, whitespacePrefix);
                continue;
            }

            if (TryHandleLinkBracket(tokens, index, linkOpeners, out var linkOpenIndex) && linkOpenIndex >= 0)
                DiscardDelimiterOpenersAfter(delimiterOpeners, linkOpenIndex);
        }
    }

    private static void ProcessDelimiter(
        IReadOnlyList<Token> tokens,
        int index,
        Stack<int> sameKindOpeners,
        Stack<LinkOpener> linkOpeners,
        int[] whitespacePrefix)
    {
        var token = tokens[index];

        if (token.CanClose() && TryCloseDelimiter(tokens, sameKindOpeners, index, whitespacePrefix))
        {
            if (token.Role == TokenRole.Closing)
                RecordClosedDelimiter(linkOpeners, token.PairIndex);

            return;
        }

        if (token.CanOpen())
            sameKindOpeners.Push(index);
    }

    private static bool TryCloseDelimiter(
        IReadOnlyList<Token> tokens,
        Stack<int> openers,
        int closeIndex,
        int[] whitespacePrefix)
    {
        DiscardUnclosableOpeners(tokens, openers, closeIndex, whitespacePrefix);

        if (openers.Count == 0)
            return false;

        var openIndex = openers.Peek();

        if (closeIndex == openIndex + 1)
        {
            openers.Pop();
            return true;
        }

        if (!IsValidPair(tokens, openIndex, closeIndex, whitespacePrefix))
            return false;

        openers.Pop();
        SetPair(tokens, openIndex, closeIndex);
        return true;
    }

    private static void DiscardUnclosableOpeners(
        IReadOnlyList<Token> tokens,
        Stack<int> openers,
        int closeIndex,
        int[] whitespacePrefix)
    {
        while (openers.Count > 0)
        {
            var openIndex = openers.Peek();

            if (!IsValidPair(tokens, openIndex, closeIndex, whitespacePrefix, assumeCloserOutsideWord: true))
                openers.Pop();
            else
                break;
        }
    }

    private static bool IsValidPair(
        IReadOnlyList<Token> tokens,
        int openIndex,
        int closeIndex,
        int[] whitespacePrefix,
        bool assumeCloserOutsideWord = false)
    {
        var hasWhitespace = HasWhitespaceBetween(openIndex, closeIndex, whitespacePrefix);
        if (!hasWhitespace)
            return true;

        if (tokens[openIndex].IsInsideWord())
            return false;

        return assumeCloserOutsideWord || !tokens[closeIndex].IsInsideWord();
    }

    private static bool HasWhitespaceBetween(int openIndex, int closeIndex, int[] whitespacePrefix)
    {
        return whitespacePrefix[closeIndex] - whitespacePrefix[openIndex + 1] > 0;
    }

    private static int[] BuildWhitespacePrefixSums(IReadOnlyList<Token> tokens)
    {
        var prefix = new int[tokens.Count + 1];

        for (var index = 0; index < tokens.Count; index++)
        {
            var hasWhitespace = ContainsWhitespace(tokens[index].Value);
            prefix[index + 1] = prefix[index] + (hasWhitespace ? 1 : 0);
        }

        return prefix;
    }

    private static bool ContainsWhitespace(string text)
    {
        foreach (var ch in text)
            if (char.IsWhiteSpace(ch))
                return true;

        return false;
    }

    private static void PairLinkBrackets(IReadOnlyList<Token> tokens)
    {
        var linkOpeners = new Stack<LinkOpener>();

        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];

            if (token.IsDelimiter && token.Role == TokenRole.Closing)
            {
                RecordClosedDelimiter(linkOpeners, token.PairIndex);
                continue;
            }

            if (TryHandleLinkBracket(tokens, index, linkOpeners, out var openIndex) && openIndex >= 0)
                DeactivateDelimitersCrossingLink(tokens, openIndex, index);
        }
    }

    private static bool TryHandleLinkBracket(
        IReadOnlyList<Token> tokens,
        int index,
        Stack<LinkOpener> linkOpeners,
        out int linkOpenIndex)
    {
        linkOpenIndex = -1;
        var token = tokens[index];

        if (token.Kind == TokenKind.LeftBracket)
        {
            linkOpeners.Push(new LinkOpener(index, index));
            return true;
        }

        if (token.Kind == TokenKind.RightBracket)
        {
            TryCloseLink(tokens, index, linkOpeners, out linkOpenIndex);
            return true;
        }

        return false;
    }

    private static bool TryCloseLink(
        IReadOnlyList<Token> tokens,
        int closeIndex,
        Stack<LinkOpener> linkOpeners,
        out int openIndex)
    {
        openIndex = -1;
        if (linkOpeners.Count == 0)
            return false;

        var (candidateOpenIndex, minDelimOpen) = linkOpeners.Pop();
        RecordClosedDelimiter(linkOpeners, minDelimOpen);

        var delimiterOpenedBeforeLink = minDelimOpen < candidateOpenIndex;
        var isLinkTextEmpty = closeIndex == candidateOpenIndex + 1;

        if (delimiterOpenedBeforeLink || isLinkTextEmpty || !HasUrlAfter(tokens, closeIndex))
            return false;

        openIndex = candidateOpenIndex;
        SetPair(tokens, openIndex, closeIndex);

        // Вложенные ссылки запрещены спецификацией: срабатывает только внутренняя ссылка
        linkOpeners.Clear();
        return true;
    }

    private static void RecordClosedDelimiter(
        Stack<LinkOpener> linkOpeners,
        int delimiterOpenIndex)
    {
        if (linkOpeners.Count == 0)
            return;

        var top = linkOpeners.Pop();
        linkOpeners.Push(new LinkOpener(top.OpenIndex, Math.Min(top.MinDelimiterOpenIndex, delimiterOpenIndex)));
    }

    private static void DiscardDelimiterOpenersAfter(
        Dictionary<TokenKind, Stack<int>> delimiterOpeners,
        int linkOpenIndex)
    {
        foreach (var stack in delimiterOpeners.Values)
            while (stack.Count > 0 && stack.Peek() > linkOpenIndex)
                stack.Pop();
    }

    private static void DeactivateDelimitersCrossingLink(IReadOnlyList<Token> tokens, int openIndex, int closeIndex)
    {
        for (var index = openIndex + 1; index < closeIndex; index++)
        {
            var token = tokens[index];
            if (token.IsDelimiter && token.Role == TokenRole.Opening && token.PairIndex > closeIndex)
                DeactivatePair(tokens, token);
        }
    }

    private static void ResetLinks(IReadOnlyList<Token> tokens)
    {
        foreach (var token in tokens)
            if (token.Kind is TokenKind.LeftBracket or TokenKind.RightBracket)
                token.Deactivate();
    }

    private static bool HasUrlAfter(IReadOnlyList<Token> tokens, int closeIndex)
    {
        return closeIndex + 1 < tokens.Count && tokens[closeIndex + 1].Kind == TokenKind.LinkUrl;
    }

    private static bool DeactivateCrossingDelimiters(IReadOnlyList<Token> tokens)
    {
        var openers = new Stack<int>();
        var hasCrossings = false;

        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];
            if (!token.IsDelimiter)
                continue;

            if (token.Role == TokenRole.Opening)
            {
                openers.Push(index);
                continue;
            }

            if (token.Role != TokenRole.Closing)
                continue;

            var openIndex = token.PairIndex;
            var isCrossed = false;

            while (openers.Peek() != openIndex)
            {
                var innerIndex = openers.Pop();
                isCrossed = true;
                DeactivatePair(tokens, innerIndex);
            }

            openers.Pop();

            if (isCrossed)
            {
                DeactivatePair(tokens, openIndex);
                hasCrossings = true;
            }
        }

        return hasCrossings;
    }

    private static void DeactivateStrongInsideEm(IReadOnlyList<Token> tokens)
    {
        var emDepth = 0;

        foreach (var token in tokens)
        {
            if (token.Kind == TokenKind.Underscore)
            {
                if (token.Role == TokenRole.Opening)
                    emDepth++;
                else if (token.Role == TokenRole.Closing)
                    emDepth--;

                continue;
            }

            if (token.Kind == TokenKind.DoubleUnderscore && token.Role == TokenRole.Opening && emDepth > 0)
                DeactivatePair(tokens, token);
        }
    }

    private static void SetPair(IReadOnlyList<Token> tokens, int openIndex, int closeIndex)
    {
        tokens[openIndex].Role = TokenRole.Opening;
        tokens[openIndex].PairIndex = closeIndex;
        tokens[closeIndex].Role = TokenRole.Closing;
        tokens[closeIndex].PairIndex = openIndex;
    }

    private static void DeactivatePair(IReadOnlyList<Token> tokens, int openIndex)
    {
        DeactivatePair(tokens, tokens[openIndex]);
    }

    private static void DeactivatePair(IReadOnlyList<Token> tokens, Token opener)
    {
        tokens[opener.PairIndex].Deactivate();
        opener.Deactivate();
    }

    private readonly record struct LinkOpener(int OpenIndex, int MinDelimiterOpenIndex);
}