using Markdown.TagInfo;
using Markdown.TokenInfo;
using Markdown.HtmlTools;
using Markdown.CandidateInfo;

namespace Markdown.MarkdownParsers;

internal class PairedTagsParser(string tag, Tag tagType) : IMarkdownParser
{
    private readonly int tagLength = tag.Length;

    public IEnumerable<Token> Parse(string text)
    {
        var tokens = new List<Token>();
        var openedTags = new Stack<Candidate>();
        var startIndex = 0;

        do
        {
            var foundTagIndex = GetIndexOfFirstUnescapedTag(text, startIndex);

            if (foundTagIndex == -1)
            {
                break;
            }

            startIndex = foundTagIndex + tag.Length;
            HandleFoundTagAtIndex(foundTagIndex, text, openedTags, tokens);
        }
        while (startIndex < text.Length);

        return tokens.FindAll(token => token != null);
    }

    private int GetIndexOfFirstUnescapedTag(string text, int startIndex)
    {
        while (true)
        {
            var firstTagStartIndex = text.IndexOf(tag, startIndex, StringComparison.Ordinal);

            if (firstTagStartIndex == -1)
            {
                return -1;
            }

            if (Checks.IsEscaped(text, firstTagStartIndex))
            {
                startIndex++;
                continue;
            }

            return firstTagStartIndex;
        }
    }

    private void HandleFoundTagAtIndex(int position, string text, Stack<Candidate> openedTags, List<Token> tokens)
    {
        var candidate = new Candidate
        {
            Position = position,
            EdgeType = openedTags.Count > 0
                ? GetClosingTagState(text, position)
                : GetOpeningTagState(text, position)
        };

        if (candidate.EdgeType == EdgeType.NotSuitable)
        {
            return;
        }

        if (openedTags.Count > 0)
        {
            var opened = openedTags.Pop();

            if (candidate.Position - opened.Position - tagLength > 0)
            {
                var (open, close) = TryCreateTagsPair(text, opened, candidate);
                tokens.Add(open);
                tokens.Add(close);
            }
        }
        else
        {
            openedTags.Push(candidate);
        }
    }

    private (Token, Token) TryCreateTagsPair(string text, Candidate open, Candidate close)
    {
        if (Checks.CanSelect(text, open, close))
        {
            return
                (new Token(tagType, open.Position, TagType.Open, tagLength),
                    new Token(tagType, close.Position, TagType.Close, tagLength));
        }

        return (null, null)!;
    }

    private static EdgeType GetOpeningTagState(string text, int position)
    {
        if (Checks.IsBeforeSpace(text, position))
        {
            return EdgeType.NotSuitable;
        }

        if (Checks.IsAfterSpace(text, position))
        {
            return EdgeType.Edge;
        }

        return EdgeType.Middle;
    }

    private EdgeType GetClosingTagState(string text, int position)
    {
        if (Checks.IsAfterSpace(text, position))
        {
            return EdgeType.NotSuitable;
        }

        if (Checks.IsBeforeSpace(text, position + tag.Length - 1))
        {
            return EdgeType.Edge;
        }

        return EdgeType.Middle;
    }
}