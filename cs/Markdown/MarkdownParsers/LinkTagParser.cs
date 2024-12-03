using Markdown.TagInfo;
using Markdown.TokenInfo;

namespace Markdown.MarkdownParsers;

internal class LinkTagParser : IMarkdownParser
{
    private const Tag TagType = Tag.Link;

    public IEnumerable<Token> Parse(string text)
    {
        var tokens = new List<Token>();
        var tagsPair = TryFindLinkTagPosition(text, 0);

        while (tagsPair is { Item1: not null, Item2: not null })
        {
            tokens.Add(tagsPair.Item1);
            tokens.Add(tagsPair.Item2);

            tagsPair = TryFindLinkTagPosition(text, tagsPair.Item2.Position);
        }

        return tokens;
    }

    private static (Token, Token) TryFindLinkTagPosition(string text, int startIndex)
    {
        var start = text.IndexOf('[', startIndex);

        if (start == -1)
        {
            return (null, null)!;
        }

        var mid = text.IndexOf("](", start, StringComparison.Ordinal);
        var end = text.IndexOf(')', start);

        if (start < end && mid < end)
        {
            return CreateLinkTokenPair(start, end - start + 1);
        }

        return (null, null)!;
    }

    private static (Token, Token) CreateLinkTokenPair(int startIndex, int sequenceLength)
    {
        return
        (
            new Token(TagType, startIndex, TagInfo.TagType.Open, sequenceLength),
            new Token(TagType, startIndex + sequenceLength - 1, TagInfo.TagType.Close, 0)
        );
    }
}