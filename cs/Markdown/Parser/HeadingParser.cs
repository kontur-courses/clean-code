using Markdown.Parsing;

namespace Markdown;

public static class HeadingParser
{
    public static bool IsStartOfHeadingLine(TokenIndexer indexer)
    {
        var save = indexer.Index;

        while (!indexer.End && indexer.Is(TokenType.Whitespace))
            indexer.Consume();

        if (indexer.Is(TokenType.Hash))
            indexer.Consume();

        if (!indexer.Is(TokenType.Whitespace))
        {
            indexer.Index = save;
            return false;
        }

        indexer.Index = save;
        return true;
    }

    public static HeadingNode ParseHeading(TokenIndexer indexer)
    {
        while (!indexer.End && indexer.Is(TokenType.Whitespace))
            indexer.Consume();

        var level = 0;
        while (!indexer.End && indexer.Is(TokenType.Hash))
        {
            indexer.Consume();
            level++;
        }

        if (indexer.Is(TokenType.Whitespace))
            indexer.Consume();

        var inner = InlineParser.ParseUntilEndOfLineOrEof(indexer);

        if (!indexer.End && indexer.Is(TokenType.EndOfLine))
            indexer.Consume();

        return new HeadingNode(inner);
    }
}