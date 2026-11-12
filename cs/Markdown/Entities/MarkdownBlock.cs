using Markdown.Interfaces;

namespace Markdown.Entities;

public sealed class MarkdownBlock
{
    public required IBlockTag Tag { get; set; }

    public required string Content { get; set; }
}