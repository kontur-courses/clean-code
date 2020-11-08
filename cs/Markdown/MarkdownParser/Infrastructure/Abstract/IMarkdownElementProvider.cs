using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser.Infrastructure.Abstract
{
    public interface IMarkdownElementProvider
    {
        bool TryParse(MarkdownElementContext context, out MarkdownElement element);
    }
}