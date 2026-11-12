using Markdown.Interfaces;

namespace Markdown.Entities;

public sealed class InlineMarker(IInlineTag tag, int start, bool canOpen, bool canClose)
{
    public IInlineTag Tag { get; } = tag;

    public int Start { get; } = start;

    public int Length => Tag.Marker.Length;

    public bool CanOpen { get; } = canOpen;

    public bool CanClose { get; } = canClose;

    public InlineMarker? Pair { get; set; }
}