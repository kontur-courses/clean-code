namespace MarkupConverter.AST.Blocks;

public abstract class BlockContainerNode
{
    public List<Block> Blocks { get; }
    
    protected BlockContainerNode(List<Block> blocks) => Blocks = blocks;
}
