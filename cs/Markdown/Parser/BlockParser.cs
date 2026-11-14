using Markdown.Node;
using Markdown.Node.Type;
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
        var blocks = new List<BlockNode>();
        var lines = markdown.Split(["\r\n", "\n"], StringSplitOptions.None);

        foreach (var line in lines)
        {
            var isHeader = line.StartsWith("# ", StringComparison.Ordinal);

            var content = isHeader ? line[2..] : line;
            var blockType = isHeader ? BlockType.Header1 : BlockType.Paragraph;

            var tokens = _tokenizer.Tokenize(content);
            var children = _inlineParser.Parse(tokens);

            blocks.Add(new BlockNode(blockType, children));
        }

        return blocks;
    }
}