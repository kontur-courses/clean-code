using MarkupConverter.AST.Blocks;
using MarkupConverter.Parsers.InlineParsers;

namespace MarkupConverter.Parsers.BlockParsers.OpenBlocks;

public class ParagraphOpenBlock : IOpenBlock
{
    public string Content { get; private set; }
    public Type BlockType => typeof(Paragraph);
    
    public ParagraphOpenBlock(string content)
    {
        Content = content;
    }

    public bool CanAccept(IOpenBlock block)
    {
        return block.BlockType == BlockType;
    }

    public void Accept(IOpenBlock block)
    {
        Content += ' ' + block.Content;
    }

    public Block Close(IInlineParser parser)
    {
        return new Paragraph(parser.Parse(Content));
    }
}