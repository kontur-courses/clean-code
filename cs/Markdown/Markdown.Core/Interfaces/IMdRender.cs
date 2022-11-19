using Markdown.Core.Entities;

namespace Markdown.Core.Interfaces
{
    public interface IMdRender
    {
        string Render(IEnumerable<TagNode> nodes);
    }
}