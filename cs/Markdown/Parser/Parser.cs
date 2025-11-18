namespace Markdown.Parsing;

public class Parser
{
    public DocumentNode Parse(List<Token> tokens)
    {
        var indexer = new TokenIndexer(tokens);
        var nodes = ParseDocument(indexer);
        return new DocumentNode(nodes);
    }

    private List<INode> ParseDocument(TokenIndexer indexer)
    {
        var nodes = new List<INode>();

        while (!indexer.End)
        {
            if (HeadingParser.IsStartOfHeadingLine(indexer))
            {
                var heading = HeadingParser.ParseHeading(indexer);
                nodes.Add(heading);
                continue;
            }

            var lineNodes = InlineParser.ParseUntilEndOfLineOrEof(indexer);
            nodes.AddRange(lineNodes);

            if (indexer.Is(TokenType.EndOfLine))
                indexer.Consume();
        }

        return nodes;
    }
}