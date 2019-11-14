using Markdown.Core.Tokens;

namespace Markdown.Core.HTMLTags
{
    public interface IHTMLTagToken
    {
        bool IsOpen { get; }
    }
}