using System.Text;
using Markdown.Parsing;

namespace Markdown;

public static class InlineParser
{
    public static List<INode> ParseUntilEndOfLineOrEof(TokenIndexer indexer)
    {
        return Parse(indexer, null);
    }

    internal static List<INode> ParseUntil(TokenIndexer indexer, Func<TokenIndexer, bool> shouldStop)
    {
        return Parse(indexer, shouldStop);
    }

    private static List<INode> Parse(TokenIndexer indexer, Func<TokenIndexer, bool>? shouldStop)
    {
        var nodes = new List<INode>();
        var underscoreParser = new UnderscoreParser(indexer);

        while (!indexer.End && !ShouldStop(shouldStop, indexer))
        {
            if (indexer.Is(TokenType.DoubleUnderscore))
            {
                underscoreParser.ParseUnderscore(isStrong: true, nodes);
                continue;
            }

            if (indexer.Is(TokenType.Underscore))
            {
                underscoreParser.ParseUnderscore(isStrong: false, nodes);
                continue;
            }

            if (LinkParser.CanStartLink(indexer))
            {
                if (LinkParser.TryParseLink(indexer, nodes))
                    continue;

                nodes.Add(new TextNode(indexer.Consume().Value));
                continue;
            }

            nodes.Add(ParseText(indexer, shouldStop));
        }

        return nodes;
    }
    
    private static bool ShouldStop(Func<TokenIndexer, bool>? predicate, TokenIndexer indexer)
    {
        return predicate == null ? indexer.Is(TokenType.EndOfLine) : predicate(indexer);
    }

    private static TextNode ParseText(TokenIndexer indexer, Func<TokenIndexer, bool>? shouldStop)
    {
        string? firstValue = null;
        StringBuilder? builder = null;

        while (!indexer.End &&
               !ShouldStop(shouldStop, indexer) &&
               !indexer.Is(TokenType.EndOfLine) &&
               !indexer.Is(TokenType.Underscore) &&
               !indexer.Is(TokenType.DoubleUnderscore))
        {
            if (firstValue != null && LinkParser.CanStartLink(indexer))
                break;

            if (indexer.Is(TokenType.Escape))
            {
                var escaped = ParseEscapedText(indexer);
                AppendValue(ref firstValue, ref builder, escaped);
                continue;
            }

            var value = indexer.Consume().Value;
            AppendValue(ref firstValue, ref builder, value);
        }

        var text = builder != null
            ? builder.ToString()
            : firstValue ?? string.Empty;

        return new TextNode(text);
    }

    private static void AppendValue(ref string? firstValue, ref StringBuilder? builder, string value)
    {
        if (firstValue == null)
        {
            firstValue = value;
            return;
        }

        builder ??= new StringBuilder(firstValue);
        builder.Append(value);
    }

    private static string ParseEscapedText(TokenIndexer indexer)
    {
        indexer.Consume();

        if (indexer.End || indexer.Is(TokenType.EndOfLine))
            return "\\";

        return indexer.Consume().Value;
    }
}
