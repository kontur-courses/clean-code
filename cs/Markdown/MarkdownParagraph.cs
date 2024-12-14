using Markdown.MarkdownTags;
using Markdown.Tokens;

namespace Markdown;

public record MarkdownParagraph
{
    public required List<IToken> Tokens { get; init; }
    
    public required List<MarkdownTag> Tags { get; init; }
}