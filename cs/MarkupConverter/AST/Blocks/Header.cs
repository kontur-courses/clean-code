using MarkupConverter.AST.Inlines;

namespace MarkupConverter.AST.Blocks;

public class Header : Block
{
    public int Level { get; }
    
    public Header(List<Inline> inlines, int level) : base(inlines)
    {
        Level = level;
    }
}