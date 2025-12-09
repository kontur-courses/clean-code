using MarkupConverter.AST.Blocks;
using MarkupConverter.Parsers.InlineParsers;

namespace MarkupConverter.Parsers.BlockParsers.OpenBlocks;

public class HeaderOpenBlock : IOpenBlock
{
    private readonly int level;
    public string Content { get; }
    public Type BlockType => typeof(Header);

    public HeaderOpenBlock(string content, int level)
    {
        Content = content;
        this.level = level;
    }

    public bool CanAccept(IOpenBlock block)
    {
        return false;
    }

    public void Accept(IOpenBlock block)
    {
    }

    public Block Close(IInlineParser parser)
    {
        return new Header(parser.Parse(Content), level);
    }
}