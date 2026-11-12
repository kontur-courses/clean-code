using Markdown.Interfaces;

namespace Markdown.Tags;

public sealed class StrongTag : IInlineTag
{
    public string Marker => "__";

    public string RenderOpen() => "<strong>";

    public string RenderClose() => "</strong>";

    public bool CanContain(IInlineTag inner) => true;
}