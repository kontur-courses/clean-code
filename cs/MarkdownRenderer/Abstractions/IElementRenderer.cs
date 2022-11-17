namespace MarkdownRenderer.Abstractions;

public interface IElementRenderer
{
    Type RenderingElementType { get; }
    string Render(IElement elem, IReadOnlyDictionary<Type, IElementRenderer> renderers);
}