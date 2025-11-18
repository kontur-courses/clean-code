using Markdown.Parsing;

namespace Markdown;

public class UnderscoreParser(TokenIndexer indexer)
{
    private readonly UnderscoreStack stack = new();
    private readonly UnderscoreValidator validator = new(indexer);

    public void ParseUnderscore(bool isStrong, List<INode> nodes)
    {
        var markerIndex = indexer.Index;
        var canOpen = validator.CanOpen(markerIndex);
        var canClose = validator.CanClose(markerIndex);
        var suppressStrong = isStrong && stack is { SingleDepth: > 0, DoubleDepth: 0 };

        var token = indexer.Consume();
        var placeholderIndex = nodes.Count;
        nodes.Add(new TextNode(token.Value));

        var closed = canClose && TryClose(isStrong, placeholderIndex, markerIndex, nodes);

        if (!closed && canOpen)
            stack.Push(isStrong, markerIndex, placeholderIndex, nodes.Count, suppressStrong);
    }

    private bool TryClose(bool isStrong, int closingPlaceholderIndex, int closingTokenIndex, List<INode> nodes)
    {
        var match = stack.FindMatchingUnderscore(isStrong, closingTokenIndex, validator);
        if (match == null)
            return false;

        if (match.Value.IsIntersection)
        {
            stack.RemoveFrom(0);
            return false;
        }

        var openerIndex = match.Value.Index;
        var opener = match.Value.Underscore!;

        if (opener.IsSuppressed || !validator.HasValidContent(opener.TokenIndex, closingTokenIndex))
        {
            stack.RemoveAt(openerIndex);
            return false;
        }

        NodeBuilder.BuildNode(opener, closingPlaceholderIndex, nodes);
        stack.RemoveFrom(openerIndex);
        return true;
    }
}