using System.Text;

namespace Markdown.Parsing.Nodes;

public interface INode
{
    void RenderHtml(StringBuilder sb);
}