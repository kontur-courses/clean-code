using Markdown.Interfaces;

namespace Markdown.Tokens;

public class MarkdownToken
{
    public string? TextContent { get; }
    public IMarkdownTag? AssociatedTag { get; }

    public MarkdownToken(IMarkdownTag? associatedTag = null, string? textContent = null)
    {
        if (associatedTag == null && textContent == null)
        {
            throw new ArgumentException("Хотя бы один из параметров должен иметь ненулевое значение.");
        }

        TextContent = textContent ?? string.Empty;
        AssociatedTag = associatedTag;
    }
}