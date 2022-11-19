using System.Text;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsRenderers;
using IElementRenderer = MarkdownRenderer.Abstractions.ElementsRenderers.IElementRenderer;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public abstract class HtmlElementRenderer<TElem> : IElementRenderer
    where TElem : IElement
{
    public Type RenderingElementType { get; } = typeof(TElem);
    protected abstract string TagText { get; }
    protected virtual string OpeningTag => $"<{TagText}>";
    protected virtual string ClosingTag => $"</{TagText}>";

    string IElementRenderer.Render(IElement elem, IRenderersProvider renderersProvider) =>
        elem switch
        {
            TElem tElement => Render(tElement, renderersProvider),
            null => throw new ArgumentNullException(nameof(elem)),
            _ => throw new InvalidOperationException(
                $"Unable to render element of type: {elem.GetType()} using this renderer")
        };

    public virtual string Render(TElem elem, IRenderersProvider renderersProvider)
    {
        var result = new StringBuilder();

        result.Append(OpeningTag);

        foreach (var nestedElement in elem.NestedElements)
        {
            var renderer = renderersProvider.GetElementRenderer(nestedElement);
            result.Append(renderer.Render(nestedElement, renderersProvider));
        }

        result.Append(ClosingTag);

        return result.ToString();
    }
}