using System.Text;
using MarkdownProcessor.Markdown;

namespace MarkdownProcessor;

public interface IRenderer
{
    public string Render(IEnumerable<ITag> tags, StringBuilder text);
}