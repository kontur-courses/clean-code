using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsRenderers;

namespace MarkdownRenderer.Implementations;

public class DefaultLineRenderer : ILineRenderer
{
    private readonly IRenderersProvider _renderersProvider;

    public DefaultLineRenderer(IRenderersProvider renderersProvider)
    {
        _renderersProvider = renderersProvider;
    }

    public string RenderLine(IElement lineElement) =>
        _renderersProvider.GetElementRenderer(lineElement).Render(lineElement, _renderersProvider);
}