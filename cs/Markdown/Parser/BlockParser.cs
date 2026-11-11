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
            if (line.StartsWith("# ", StringComparison.Ordinal))
            {
                var headerContent = line[2..];
                var tokens = _tokenizer.Tokenize(headerContent);
                var children = _inlineParser.Parse(tokens);
                blocks.Add(new BlockNode(BlockType.Header1, children));
            }
            else
            {
                var tokens = _tokenizer.Tokenize(line);
                var children = _inlineParser.Parse(tokens);
                blocks.Add(new BlockNode(BlockType.Paragraph, children));
            }
        }

        return blocks;
    }
}