using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions.ElementsRenderers;

public interface IElementRenderer
{
    Type RenderingElementType { get; }
    string Render(IElement elem, IRenderersProvider renderersProvider);
}