using MarkupConverter.AST.Inlines;

namespace MarkupConverter.AST.Blocks;

public abstract class Block : Node
{
    public List<Inline> Inlines { get; }
    
    protected Block(List<Inline> inlines) => Inlines = inlines;
    
}