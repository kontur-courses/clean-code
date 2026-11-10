using Markdown.Tokenization.Tokens;
using Markdown.Tokenization.Tokens.Abstractions;
using Markdown.Tokenization.Tokens.Implementations;

namespace Markdown.Tokenization;

public class TokenResolver
{
    public ResolvedTokens Resolve(UnresolvedTokens unresolved)
    {
        var tokens = unresolved.ToList();
        InitializeContext(tokens);

        var pairs = FindPairs(tokens)
            .OrderBy(pair => pair.Start)
            .ToList();

        InvalidateCrossings(pairs);
        InvalidateStrongInsideEm(pairs);
        SetRoles(pairs);

        return new ResolvedTokens(tokens);
    }

    private static void InitializeContext(List<Token> tokens)
    {
        var segment = 0;

        foreach (var token in tokens)
        {
            if (token is TextToken { ContainsWordSeparator: true })
                segment++;

            if (token is PairToken pairToken)
                pairToken.Context = new ResolvingContext(PairRole.Text, segment);
        }
    }

    private static List<Pair> FindPairs(IEnumerable<Token> tokens)
    {
        var pairs = new List<Pair>();
        var openedEm = new Stack<PairToken>();
        var openedStrong = new Stack<PairToken>();

        foreach (var current in tokens.OfType<PairToken>())
        {
            var openedStack = current is EmToken ? openedEm : openedStrong;

            while (openedStack.TryPeek(out var candidate) &&
                candidate.IsInsideWord &&
                candidate.Context!.SegmentId != current.Context!.SegmentId)
                openedStack.Pop();

            if (current.CanClose &&
                openedStack.TryPeek(out var opening) &&
                IsValidPair(opening, current))
            {
                openedStack.Pop();
                pairs.Add(new Pair(opening, current));
                continue;
            }

            if (current.CanOpen)
                openedStack.Push(current);
        }

        return pairs;
    }

    private static void InvalidateCrossings(IEnumerable<Pair> pairs)
    {
        var activeQueue = new PriorityQueue<Pair, int>();
        var notInvalidatedQueue = new PriorityQueue<Pair, int>();

        foreach (var pair in pairs)
        {
            RemoveFinished(activeQueue, pair.Start);
            RemoveFinished(notInvalidatedQueue, pair.Start);

            if (activeQueue.TryPeek(out _, out var earliestEnd) && earliestEnd < pair.End)
                pair.IsInvalid = true;

            while (notInvalidatedQueue.TryPeek(out _, out var earlierEnd) && earlierEnd < pair.End)
                notInvalidatedQueue.Dequeue().IsInvalid = true;

            activeQueue.Enqueue(pair, pair.End);

            if (!pair.IsInvalid)
                notInvalidatedQueue.Enqueue(pair, pair.End);
        }
    }

    private static void RemoveFinished(PriorityQueue<Pair, int> queue, int position)
    {
        while (queue.TryPeek(out _, out var end) && end <= position)
            queue.Dequeue();
    }

    private static void InvalidateStrongInsideEm(IEnumerable<Pair> pairs)
    {
        var openedEm = new Stack<Pair>();

        foreach (var pair in pairs)
        {
            while (openedEm.TryPeek(out var em) &&
                   em.Closing.Start < pair.Opening.Start)
                openedEm.Pop();

            if (pair.IsInvalid)
                continue;

            if (pair.Opening is EmToken)
                openedEm.Push(pair);
            else if (pair.Opening is StrongToken && openedEm.Count > 0)
                pair.IsInvalid = true;
        }
    }

    private static void SetRoles(IEnumerable<Pair> pairs)
    {
        foreach (var pair in pairs)
        {
            if (pair.IsInvalid)
                continue;

            pair.Opening.Context!.Role = PairRole.Open;
            pair.Closing.Context!.Role = PairRole.Close;
        }
    }

    private static bool IsValidPair(PairToken opening, PairToken closing)
    {
        if (opening.Start + opening.Length == closing.Start)
            return false;

        if (opening.IsInsideWord || closing.IsInsideWord)
            return opening.Context!.SegmentId == closing.Context!.SegmentId;

        return true;
    }
}
