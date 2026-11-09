using System.Text;
using Markdown.Domain;

namespace Markdown;

public static class MarkdownParser
{
    private const string HeaderPrefix = "# ";
    private const int EmphasisDelimiterLenght = 1;
    private const int StrongDelimiterLenght = 2;

    public static IReadOnlyList<Node> Parse(string markdown)
    {
        var nodes = new List<Node>();

        using var reader = new StringReader(markdown);

        while(reader.ReadLine() is {} line)
        {
            nodes.Add(ParseLine(line));
        }

        return nodes;
    }

    private static Node ParseLine(string line)
    {
        var isHeader = line.StartsWith(HeaderPrefix);
        var startIndex = isHeader ? HeaderPrefix.Length : 0;

        var tokens = InlineTokenizer.Tokenize(line, startIndex);
        var pairs = FindDelimiterTokenPairs(tokens, line);
        var children = BuildNodes(
            line, startIndex, [.. tokens.OfType<EscapeToken>()], pairs);

        return isHeader
            ? new HeaderNode { Children = children }
            : new ParagraphNode { Children = children };
    }

    private static IReadOnlyList<Node> BuildNodes(
        string text,
        int startIndex,
        IReadOnlyList<EscapeToken> escapeTokens,
        IReadOnlyList<DelimiterPair> pairs)
    {
        var openingIndexes = pairs.ToDictionary(p => p.Opening.Index);
        var escapeIndexes = escapeTokens.Select(x => x.Index).ToHashSet();

        var nodes = new List<Node>();
        var parents = new Stack<(DelimiterPair Pair, List<Node> Nodes)>();
        var sb = new StringBuilder();

        for (var index = startIndex; index < text.Length; index++)
        {
            if (escapeIndexes.Contains(index))
            {
                sb.Append(text[index + 1]);
                index++;
                continue;
            }

            if (parents.TryPeek(out var frame)
                && index == frame.Pair.Closing.Index)
            {
                FlushText();

                ContainerNode wrapper = frame.Pair.Opening.Length == EmphasisDelimiterLenght
                    ? new EmphasisNode { Children = nodes }
                    : new StrongNode { Children = nodes };

                parents.Pop();
                nodes = frame.Nodes;
                nodes.Add(wrapper);

                index += frame.Pair.Closing.Length - 1;
                continue;
            }

            if (openingIndexes.TryGetValue(index, out var pair))
            {
                FlushText();

                parents.Push((pair, nodes));
                nodes = [];

                index += pair.Opening.Length - 1;
                continue;
            }

            sb.Append(text[index]);
        }

        FlushText();
        return nodes;

        void FlushText()
        {
            if (sb.Length == 0)
            {
                return;
            }

            nodes.Add(new TextNode(sb.ToString()));
            sb.Clear();
        }
    }

    internal static IReadOnlyList<DelimiterPair> FindDelimiterTokenPairs(
        IReadOnlyList<Token> tokens,
        string text)
    {
        var pairs = new List<DelimiterPair>();

        var stackOpenStrong = new Stack<DelimiterRunToken>();
        var stackOpenEmphasis = new Stack<DelimiterRunToken>();

        foreach (var token in tokens.OfType<DelimiterRunToken>())
        {
            var preferredEmphasis = stackOpenEmphasis.TryPeek(out var openEmphasis)
                                    && (!stackOpenStrong.TryPeek(out var openStrong) || openEmphasis.Index > openStrong.Index);

            foreach (var delimiter in SplitDelimiter(token, preferredEmphasis))
            {
                var stack = delimiter.Length == StrongDelimiterLenght ? stackOpenStrong : stackOpenEmphasis;

                if (delimiter.CanClose
                    && stack.TryPop(out var opening))
                {
                    pairs.Add(new DelimiterPair(opening, delimiter));
                }
                else if (delimiter.CanOpen)
                {
                    stack.Push(delimiter);
                }
            }
        }

        return FilterValidPairs(pairs, text);
    }

    private static IReadOnlyList<DelimiterPair> FilterValidPairs(
        IReadOnlyList<DelimiterPair> pairs, string text)
    {
        return
        [
            .. pairs.Where(pair =>
                (pair.Opening.Index + pair.Opening.Length != pair.Closing.Index)
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
            (start < otherStart && otherStart < end && end < otherEnd) ||
            (otherStart < start && start < otherEnd && otherEnd < end);

        var strongInsideEmphasis =
            pair.Opening.Length == StrongDelimiterLenght &&
            other.Opening.Length == EmphasisDelimiterLenght &&
            otherStart < start &&
            end < otherEnd;

        var isNestedWithinSameLength =
            pair.Opening.Length == other.Opening.Length &&
            other.Opening.Index < pair.Opening.Index &&
            other.Closing.Index > pair.Closing.Index;

        return crosses || strongInsideEmphasis || isNestedWithinSameLength;
    }

    private static bool IsInvalidWordPair(DelimiterPair pair, string text)
    {
        var openingInsideWord = IsInsideWord(pair.Opening, text);
        var closingInsideWord = IsInsideWord(pair.Closing, text);

        var betweenText = text.AsSpan(
            pair.Opening.Index + pair.Opening.Length,
            pair.Closing.Index - pair.Opening.Index - pair.Opening.Length);

        return (openingInsideWord || closingInsideWord)
               &&  betweenText.IndexOf(' ') >= 0;
    }

    private static bool IsInsideWord(Token token, string text)
    {
        var start = token.Index;
        var end = start + token.Length;

        return start > 0 &&
               end < text.Length &&
               char.IsLetter(text[start - 1]) &&
               char.IsLetter(text[end]);
    }

    private static List<DelimiterRunToken> SplitDelimiter(
        DelimiterRunToken delimiterRun,
        bool preferredEmphasisFirst)
    {
        var result = new List<DelimiterRunToken>();
        var index = delimiterRun.Index;
        var remaining = delimiterRun.Length;

        if (remaining % 2 != 0 && preferredEmphasisFirst)
        {
            result.Add(delimiterRun with { Index = index, Length = 1 });
            index++;
            remaining--;
        }

        while (remaining >= 2)
        {
            result.Add(delimiterRun with { Index = index, Length = 2 });
            index += 2;
            remaining -= 2;
        }

        if (remaining == 1)
        {
            result.Add(delimiterRun with { Index = index, Length = 1 });
        }

        return result;
    }
}