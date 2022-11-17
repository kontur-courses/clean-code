using System.Text;
using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public abstract class HtmlElementRenderer<TElem> : IElementRenderer
    where TElem : IElement
{
    public Type RenderingElementType { get; } = typeof(TElem);
    protected abstract string TagText { get; }
    protected virtual string OpeningTag => $"<{TagText}>";
    protected virtual string ClosingTag => $"</{TagText}>";

    string IElementRenderer.Render(IElement elem, IReadOnlyDictionary<Type, IElementRenderer> renderers) =>
        elem switch
        {
            null => throw new ArgumentNullException(nameof(elem)),
            TElem tElement => Render(tElement, renderers),
            _ => throw new InvalidOperationException($"Unable to render element of type: {elem.GetType()}")
        };

    public virtual string Render(TElem elem, IReadOnlyDictionary<Type, IElementRenderer> renderers)
    {
        var result = new StringBuilder();

        result.Append(OpeningTag);
        foreach (var nestedElement in elem.NestedElements)
        {
            if (!renderers.TryGetValue(nestedElement.GetType(), out var renderer))
                throw new InvalidOperationException($"Unable to render element of type: {elem.GetType()}");
            result.Append(renderer.Render(nestedElement, renderers));
        }

        result.Append(ClosingTag);

        return result.ToString();
    }
}