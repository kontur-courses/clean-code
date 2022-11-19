using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsRenderers;

namespace MarkdownRenderer.Implementations;

public class DefaultRenderersProvider : IRenderersProvider
{
    private readonly IReadOnlyDictionary<Type, IElementRenderer> _renderers;

    public DefaultRenderersProvider(IEnumerable<IElementRenderer> renderers)
    {
        _renderers = renderers.ToDictionary(renderer => renderer.RenderingElementType);
    }

    public IElementRenderer GetElementRenderer(IElement element) =>
        GetElementRenderer(element.GetType());

    public IElementRenderer GetElementRenderer(Type elementType)
    {
        if (!elementType.IsAssignableTo(typeof(IElement)))
            throw new ArgumentException(
                $"Unable to render! Type should be assignable to {nameof(IElement)}, but was {elementType.Name}");

        if (!_renderers.TryGetValue(elementType, out var renderer))
            throw new ArgumentException($"No renderer for type: {elementType.Name}");

        return renderer;
    }
}