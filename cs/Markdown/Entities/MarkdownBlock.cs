using Markdown.Enums;

namespace Markdown.Entities;

public sealed class MarkdownBlock
{
    public required MarkdownBlockType Type { get; set; }
    
    public required string Content { get; set; }
}