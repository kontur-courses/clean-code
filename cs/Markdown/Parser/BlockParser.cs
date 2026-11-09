using Markdown.Node;
using Markdown.Parser.Interfaces;
using Markdown.Tokenizer.Interfaces;

namespace Markdown.Parser;

public class BlockParser : IBlockParser
{
    private readonly ITokenizer _tokenizer;
    private readonly IInlineParser _inlineParser;

    public BlockParser(ITokenizer tokenizer, IInlineParser inlineParser)
    {
        _tokenizer = tokenizer;
        _inlineParser = inlineParser;
    }

    public IReadOnlyList<BlockNode> Parse(string markdown)
    {
        throw new NotImplementedException();
    }
}