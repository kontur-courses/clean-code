using MarkdownParser.Infrastructure.Markdown.Models;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    /// <summary>
    /// Basicly you don't need to inherit from this interface directly,
    /// look at <see cref="MdElementFactory{TElem,TToken}"/>
    /// </summary>
    public interface IMdElementFactory
    {
        bool CanCreate(MarkdownElementContext context);
        MarkdownElement Create(MarkdownElementContext context);
    }
}