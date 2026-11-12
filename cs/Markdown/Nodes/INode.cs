using System.Text;

namespace Markdown.Nodes;

public interface INode
{
    void Render(StringBuilder sb);
}