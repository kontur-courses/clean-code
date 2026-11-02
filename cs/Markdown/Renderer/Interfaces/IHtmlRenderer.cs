using Markdown.Tokens;

namespace Markdown.Renderer.Interfaces;

public interface IHtmlRenderer
{
    string Render(IEnumerable<Token> tokens);
}
