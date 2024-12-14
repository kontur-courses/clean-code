using Markdown.MarkdownTags;

namespace Markdown.Tokens.MarkdownTagTokens;

public interface IMarkdownTagToken
{
    MarkdownTagType TagType { get; }
}