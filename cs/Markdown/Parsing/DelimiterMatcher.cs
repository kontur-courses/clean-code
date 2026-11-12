using Markdown.Domain;

namespace Markdown.Parsing;

internal static class DelimiterMatcher
{
    public static IReadOnlyList<DelimiterPair> FindPairs(
        IReadOnlyList<Token> tokens,
        string text)
    {
        var pairs = new List<DelimiterPair>();

        var openingEmphasis = new Stack<DelimiterRunToken>();
        var openingStrong = new Stack<DelimiterRunToken>();

        foreach (var token in tokens.OfType<DelimiterRunToken>())
        {
            var emphasisFirst =
                openingEmphasis.TryPeek(out var openEmphasis)
                && (!openingStrong.TryPeek(out var openStrong)
                    || openEmphasis.Index > openStrong.Index);

            foreach (var delimiter in SplitRun(token, emphasisFirst))
            {
                var stack = delimiter.Length == DelimiterRunToken.StrongLength
                    ? openingStrong
                    : openingEmphasis;

                var pair = delimiter.CanClose && stack.TryPeek(out var opening)
                    ? new DelimiterPair(
                        opening,
                        delimiter,
                        delimiter.Length == DelimiterRunToken.StrongLength
                            ? DelimiterType.Strong
                            : DelimiterType.Emphasis)
                    : null;

                if (pair is not null && !IsInvalidWordPair(pair, text))
                {
                    stack.Pop();
                    pairs.Add(pair);
                }
                else if (delimiter.CanOpen)
                {
                    stack.Push(delimiter);
                }
            }
        }

        return FilterPairs(pairs, text);
    }

    private static IReadOnlyList<DelimiterPair> FilterPairs(
        IReadOnlyList<DelimiterPair> pairs,
        string text)
    {
        return pairs.Where(pair =>
                pair.Opening.Index + pair.Opening.Length
                    != pair.Closing.Index
                && !pairs.Any(other => HasInvalidRelation(pair, other)))
            .ToList();
    }

    private static bool HasInvalidRelation(
        DelimiterPair pair,
        DelimiterPair other)
    {
        var start = pair.Opening.Index;
        var end = pair.Closing.Index + pair.Closing.Length;

        var otherStart = other.Opening.Index;
        var otherEnd = other.Closing.Index + other.Closing.Length;

        var crosses =
            (start < otherStart && otherStart < end && end < otherEnd)
            || (otherStart < start && start < otherEnd && otherEnd < end);

        var strongInsideEmphasis =
            pair.Opening.Length == DelimiterRunToken.StrongLength
            && other.Opening.Length == DelimiterRunToken.EmphasisLength
            && otherStart < start
            && end < otherEnd;

        var nestedSameLength =
            pair.Opening.Length == other.Opening.Length
            && otherStart < start
            && end < otherEnd;

        return crosses || strongInsideEmphasis || nestedSameLength;
    }

    private static bool IsInvalidWordPair(
        DelimiterPair pair,
        string text)
    {
        var openingInsideWord = IsInsideWord(pair.Opening, text);
        var closingInsideWord = IsInsideWord(pair.Closing, text);

        var between = text.AsSpan(
            pair.Opening.Index + pair.Opening.Length,
            pair.Closing.Index - pair.Opening.Index - pair.Opening.Length);

        return (openingInsideWord || closingInsideWord)
               && between.IndexOf(' ') >= 0;
    }

    private static bool IsInsideWord(Token token, string text)
    {
        var start = token.Index;
        var end = start + token.Length;

        return start > 0
               && end < text.Length
               && char.IsLetter(text[start - 1])
               && char.IsLetter(text[end]);
    }

    private static IEnumerable<DelimiterRunToken> SplitRun(
        DelimiterRunToken run,
        bool emphasisFirst)
    {
        var index = run.Index;
        var remaining = run.Length;

        if (remaining % DelimiterRunToken.StrongLength != 0 && emphasisFirst)
        {
            yield return run with { Index = index, Length = DelimiterRunToken.EmphasisLength };
            index += DelimiterRunToken.EmphasisLength;
            remaining -= DelimiterRunToken.EmphasisLength;
        }

        while (remaining >= DelimiterRunToken.StrongLength)
        {
            yield return run with { Index = index, Length = DelimiterRunToken.StrongLength };
            index += DelimiterRunToken.StrongLength;
            remaining -= DelimiterRunToken.StrongLength;
        }

        if (remaining == DelimiterRunToken.EmphasisLength)
        {
            yield return run with { Index = index, Length = DelimiterRunToken.EmphasisLength };
        }
    }
}