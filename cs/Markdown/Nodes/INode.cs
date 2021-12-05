using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public interface INode
    {
        public NodeCondition Condition { get; }
        public bool TryOpen(Stack<INode> parentNodes, CollectionIterator<IToken> iterator);
        public void UpdateCondition(IToken newToken);
        public void AddChild(INode child);
        public StringBuilder GetNodeBuilder();
    }
}