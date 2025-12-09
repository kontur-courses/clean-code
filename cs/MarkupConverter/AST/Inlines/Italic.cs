namespace MarkupConverter.AST.Inlines;

public class Italic : InlineLeaf
{
    public List<Inline> InlineLeaves { get; }

    public Italic(List<Inline> inlineLeaves)
    {
        InlineLeaves = inlineLeaves;
    }
}