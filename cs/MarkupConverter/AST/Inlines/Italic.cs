namespace MarkupConverter.AST.Inlines;

public class Italic : Inline
{
    public List<InlineLeaf> InlineLeaves { get; }
    
    public Italic(List<InlineLeaf> inlineLeaves) => InlineLeaves = inlineLeaves;
}