using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Nodes
{
    public class JoiningNode : INode
    {
        private List<INode> nodes = new List<INode>();

        public void AddNode(INode newNode)
        {
            nodes.Add(newNode);
        }

        public virtual StringBuilder GetNodeBuilder()
        {
            var builder = new StringBuilder();
            foreach (var node in nodes.Select(x => x.GetNodeBuilder()))
                builder.Append(node);
            return builder;
        }
    }
}