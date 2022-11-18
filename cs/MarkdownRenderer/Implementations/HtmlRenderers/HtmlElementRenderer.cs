using System.Text;
using MarkdownRenderer.Abstractions;
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
            null => throw new ArgumentNullException(nameof(elem)),
            TElem tElement => Render(tElement, renderersProvider),
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