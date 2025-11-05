using MarkupConverter.AST.Blocks;

namespace MarkupConverter.Renderers;

public interface IRenderer
{
    public string Render(Document doc);
}