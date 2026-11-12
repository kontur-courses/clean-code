using Markdown.Interfaces;

namespace Markdown.Tags;

public sealed class EmphasisTag : IInlineTag
{
    public string Marker => "_";

    public string RenderOpen() => "<em>";

    public string RenderClose() => "</em>";

    public bool CanContain(IInlineTag inner) => inner is not StrongTag;
}