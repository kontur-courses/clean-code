using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions.ElementsRenderers;

public interface IRenderersProvider
{
    public IElementRenderer GetElementRenderer(IElement element);
    public IElementRenderer GetElementRenderer(Type elementType);
}