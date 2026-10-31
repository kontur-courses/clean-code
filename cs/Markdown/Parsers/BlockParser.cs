using Markdown.Domain;

namespace Markdown.Parsers;

public class BlockParser(InlineParser inlineParser)
{
    private readonly InlineParser _inlineParser = inlineParser;

    public IReadOnlyList<BlockNode> Parse(string content)
    {
        throw new NotImplementedException();
    }

}