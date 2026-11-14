using Markdown.Node;

namespace Markdown.Renderer.Interfaces;

public interface IRenderer
{
    string Render(IReadOnlyList<BlockNode> blocks);
}