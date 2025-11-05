namespace MarkupConverter.AST.Inlines;

public class Bold : Inline
{
    public List<Inline> Inlines { get; }
    
    public Bold(List<Inline> inlines) => Inlines = inlines;
}