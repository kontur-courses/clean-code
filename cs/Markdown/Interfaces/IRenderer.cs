using Markdown.Tokens;

namespace Markdown.Interfaces;

public interface IRenderer
{
    string Render(IEnumerable<BaseToken> tokens);
}