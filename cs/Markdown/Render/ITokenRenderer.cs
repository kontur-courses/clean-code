using Markdown.Convert;
using Markdown.Token;

namespace Markdown.Render;

public interface ITokenRenderer
{
    public string Render(TokenTree tree, ITagConverter converter);
}