using Markdown.Nodes;

namespace Markdown.Parsing;

public class BlockParser
{
    private readonly InlineParser inlineParser;

    public BlockParser(InlineParser inlineParser)
    {
        inlineParser = inlineParser;
    }

    public List<INode> Parse(string text)
    {
        throw new NotImplementedException();
    }
}