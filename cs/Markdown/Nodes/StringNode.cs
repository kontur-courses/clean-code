using System.Text;

namespace Markdown.Nodes
{
    public class StringNode : INode
    {
        private readonly string value;

        public StringNode(string value)
        {
            this.value = value;
        }
        
        public StringBuilder GetNodeBuilder()
        {
            return new StringBuilder(value);
        }
    }
}