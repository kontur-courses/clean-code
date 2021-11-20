using System.Text;

namespace Markdown.Nodes
{
    public interface INode
    {
        StringBuilder GetNodeBuilder();
    }
}