using Markdown.Domain;

namespace Markdown.Parsing;

internal static class DelimiterMatcher
{
    private const int EmphasisLength = 1;
    private const int StrongLength = 2;

    public static IReadOnlyList<DelimiterPair> FindPairs(
        IReadOnlyList<Token> tokens,
        string text)
    {
        var pairs = new List<DelimiterPair>();

        var strong = new Stack<DelimiterRunToken>();
        var emphasis = new Stack<DelimiterRunToken>();

        foreach (var token in tokens.OfType<DelimiterRunToken>())
        {
            var emphasisFirst =
                emphasis.TryPeek(out var openEmphasis)
                && (!strong.TryPeek(out var openStrong)
                    || openEmphasis.Index > openStrong.Index);

            foreach (var delimiter in SplitRun(token, emphasisFirst))
            {
                var stack = delimiter.Length == StrongLength
                    ? strong
                    : emphasis;

                if (delimiter.CanClose && stack.TryPop(out var opening))
                {
                    pairs.Add(new DelimiterPair(
                        opening,
                        delimiter,
                        delimiter.Length == StrongLength
                            ? DelimiterType.Strong
                            : DelimiterType.Emphasis));
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
        return
        [
            .. pairs.Where(pair =>
                pair.Opening.Index + pair.Opening.Length
                    != pair.Closing.Index
                && !IsInvalidWordPair(pair, text)
                && !pairs.Any(other => HasInvalidRelation(pair, other)))
        ];
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
            pair.Opening.Length == StrongLength
            && other.Opening.Length == EmphasisLength
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

        if (remaining % 2 != 0 && emphasisFirst)
        {
            yield return run with { Index = index, Length = EmphasisLength };
            index++;
            remaining--;
        }

        while (remaining >= StrongLength)
        {
            yield return run with { Index = index, Length = StrongLength };
            index += StrongLength;
            remaining -= StrongLength;
        }

        if (remaining == EmphasisLength)
        {
            yield return run with { Index = index, Length = EmphasisLength };
        }
    }
}