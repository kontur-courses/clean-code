using System.Text;
using Markdown.Parsing;

namespace Markdown;

public static class LinkParser
{
    public static bool CanStartLink(TokenIndexer indexer)
    {
        return indexer.Is(TokenType.OpenBracket) || indexer.Is(TokenType.AutoLinkOpen);
    }

    public static bool TryParseLink(TokenIndexer indexer, List<INode> nodes)
    {
        var inlineLink = TryParseInlineLink(indexer);
        if (inlineLink != null)
        {
            nodes.Add(inlineLink);
            return true;
        }

        var autoLink = TryParseAutoLink(indexer);
        if (autoLink == null) return false;
        nodes.Add(autoLink);
        return true;
    }

    private static LinkNode? TryParseInlineLink(TokenIndexer indexer)
    {
        if (!indexer.Is(TokenType.OpenBracket))
            return null;

        var startIndex = indexer.Index;
        indexer.Consume();

        var textNodes = InlineParser.ParseUntil(indexer, c =>
            c.Is(TokenType.CloseBracket) || c.Is(TokenType.EndOfLine));

        if (indexer.End || indexer.Is(TokenType.EndOfLine) || !indexer.Is(TokenType.CloseBracket))
        {
            indexer.Index = startIndex;
            return null;
        }
        indexer.Consume();

        if (!indexer.Is(TokenType.OpenParen))
        {
            indexer.Index = startIndex;
            return null;
        }
        indexer.Consume();

        var (href, success) = ReadDestination(indexer);
        if (!success || string.IsNullOrEmpty(href))
        {
            indexer.Index = startIndex;
            return null;
        }
        indexer.Consume();

        if (textNodes.Count == 0)
            textNodes.Add(new TextNode(href));

        return new LinkNode(href, textNodes);
    }

    private static LinkNode? TryParseAutoLink(TokenIndexer indexer)
    {
        if (!indexer.Is(TokenType.AutoLinkOpen))
            return null;

        var startIndex = indexer.Index;
        indexer.Consume();

        var sb = new StringBuilder();

        while (!indexer.End && !indexer.Is(TokenType.AutoLinkClose))
        {
            if (indexer.Is(TokenType.EndOfLine) || indexer.Is(TokenType.Whitespace))
            {
                indexer.Index = startIndex;
                return null;
            }

            sb.Append(indexer.Consume().Value);
        }

        if (!indexer.Is(TokenType.AutoLinkClose))
        {
            indexer.Index = startIndex;
            return null;
        }
        indexer.Consume();

        var href = sb.ToString();
        if (!LinkValidation.IsValidAutoLinkHref(href))
        {
            indexer.Index = startIndex;
            return null;
        }

        var children = new List<INode> { new TextNode(href) };
        return new LinkNode(href, children);
    }

    private static (string href, bool success) ReadDestination(TokenIndexer indexer)
    {
        var sb = new StringBuilder();

        while (!indexer.End && !indexer.Is(TokenType.CloseParen))
        {
            if (indexer.Is(TokenType.EndOfLine))
                return (string.Empty, false);

            sb.Append(indexer.Consume().Value);
        }

        if (!indexer.Is(TokenType.CloseParen))
            return (string.Empty, false);

        var href = sb.ToString().Trim();
        return !LinkValidation.IsValidInlineHref(href) ? (string.Empty, false) : (href, true);
    }
}

