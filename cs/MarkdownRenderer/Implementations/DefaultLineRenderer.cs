using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsRenderers;

namespace MarkdownRenderer.Implementations;

public class DefaultLineRenderer : ILineRenderer
{
    private readonly IRenderersProvider _renderersProvider;

    public DefaultLineRenderer(IEnumerable<IElementRenderer> renderers)
    {
        _renderersProvider = new DefaultRenderersProvider(renderers);
    }

    public string RenderLine(IElement lineElement) => 
        _renderersProvider.GetElementRenderer(lineElement).Render(lineElement, _renderersProvider);
}