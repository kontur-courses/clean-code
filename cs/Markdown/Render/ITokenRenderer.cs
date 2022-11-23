using Markdown.Tags;
using Markdown.Token;

namespace Markdown.Render;

public interface ITokenRenderer
{
    public string Render(TokenTree tree, Dictionary<Tag, Tag> rules);
}