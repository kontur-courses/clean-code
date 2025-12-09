using MarkupConverter.AST.Blocks;
using MarkupConverter.Parsers.InlineParsers;

namespace MarkupConverter.Parsers.BlockParsers.OpenBlocks;

public interface IOpenBlock
{
    string Content { get; }
    Type BlockType { get; }
    bool CanAccept(IOpenBlock block);
    void Accept(IOpenBlock block);
    Block Close(IInlineParser parser);
}