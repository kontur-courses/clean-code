using MarkdownParser.Infrastructure.Markdown.Models;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    public interface IMarkdownElementFactory
    {
        bool TryCreate(MarkdownElementContext context, out MarkdownElement element);
    }
}