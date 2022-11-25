using System.Text;
using MarkdownProcessor.Tags;

namespace MarkdownProcessor.Renderer;

public interface IRenderer
{
    public string Render(IEnumerable<ITag> tags, StringBuilder text);
}