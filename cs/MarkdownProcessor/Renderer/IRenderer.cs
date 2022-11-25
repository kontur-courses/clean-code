using System.Text;
using MarkdownProcessor.Tags;

namespace MarkdownProcessor.Renderer;

public interface IRenderer
{
    public string Render(IEnumerable<Tag> tags, StringBuilder text);
}