using Markdown.Tags;
using Markdown.Token;

namespace Markdown.Render;

public interface IRenderer
{
    public string Render(TokenTree tree, Dictionary<Tag, Tag> rules);
}