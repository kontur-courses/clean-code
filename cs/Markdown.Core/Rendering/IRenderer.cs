using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Rendering;

public interface IRenderer
{
    string Render (DocumentNode document);
}