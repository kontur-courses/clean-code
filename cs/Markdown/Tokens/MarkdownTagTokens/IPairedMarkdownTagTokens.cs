using Markdown.MarkdownTags;
using Markdown.Tokens.TagTokens;

namespace Markdown.Tokens.MarkdownTagTokens;

public interface IPairedMarkdownTagTokens : IMarkdownTagToken
{
    TagToken Opening { get; }
    
    TagToken Closing { get; }

    IEnumerable<MarkdownTag> ConvertToMarkdownTags(); 
    
    bool IsIntersect(IPairedMarkdownTagTokens pairedMarkdownTagTokens);

    bool IsExternalFor(IPairedMarkdownTagTokens pairedMarkdownTagTokens);
}