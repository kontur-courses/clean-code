using MarkupConverter.AST.Inlines;

namespace MarkupConverter.AST.Blocks;

public class Paragraph : Block
{
    public Paragraph(List<Inline> inlines) : base(inlines) {}
}