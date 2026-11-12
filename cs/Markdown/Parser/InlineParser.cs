using Markdown.Node;
using Markdown.Node.Type;
using Markdown.Node.Interfaces;
using Markdown.Parser.Interfaces;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser;

public class InlineParser : IInlineParser
{
    public InlineParser()
    {
    }

    public IReadOnlyList<INode> Parse(IReadOnlyList<Token> tokens)
    {
        if (tokens.Count == 0)
            return [];

        var links = LinkMatcher.MatchLinks(tokens);
        var urlTokens = new HashSet<int>();
        foreach (var link in links.Values)
        {
            for (var j = link.CloseBracketIndex + 1; j <= link.CloseParenIndex; j++)
                urlTokens.Add(j);
        }

        var matchedUnderscores = UnderscoreTagMatcher.MatchTags(tokens, 0, tokens.Count - 1, urlTokens);
        return ParseRange(tokens, 0, tokens.Count - 1, links, matchedUnderscores);
    }

    private static IReadOnlyList<INode> ParseRange(
        IReadOnlyList<Token> tokens,
        int startIndex,
        int endIndex,
        Dictionary<int, LinkMatch> linksByStartIndex,
        Dictionary<int, int> closingIndicesByOpeningIndex)
    {
        var nodes = new List<INode>();

        var i = startIndex;

        while (i <= endIndex)
        {
            if (linksByStartIndex.TryGetValue(i, out var linkMatch) && linkMatch.CloseParenIndex <= endIndex)
            {
                var linkChildren = ParseRange(
                    tokens,
                    linkMatch.OpenBracketIndex + 1,
                    linkMatch.CloseBracketIndex - 1,
                    linksByStartIndex,
                    closingIndicesByOpeningIndex);

                nodes.Add(new LinkNode(linkMatch.Url, linkChildren));
                i = linkMatch.CloseParenIndex + 1;
                continue;
            }

            if (closingIndicesByOpeningIndex.TryGetValue(i, out var closeIndex) && closeIndex <= endIndex)
            {
                var emphasisType = tokens[i].Type == TokenType.SingleUnderscore
                    ? EmphasisType.Italic
                    : EmphasisType.Bold;

                var children = ParseRange(tokens, i + 1, closeIndex - 1, linksByStartIndex, closingIndicesByOpeningIndex);
                nodes.Add(new EmphasisNode(emphasisType, children));
                i = closeIndex + 1;
                continue;
            }

            nodes.Add(new TextNode(tokens[i].Value));
            i++;
        }

        return nodes;
    }
}