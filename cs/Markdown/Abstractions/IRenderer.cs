using Markdown.Primitives;

namespace Markdown.Abstractions;

public interface IRenderer
{
    string Render(IEnumerable<TagNode> tagNodes);
}