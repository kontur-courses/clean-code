using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public interface INode
    {
        public NodeCondition Condition { get; }
        public bool TryOpen(Stack<INode> parentNodes, List<IToken> tokens, ref int parentTokenPosition);
        public void AddChild(INode child);
        public void UpdateCondition(IToken newToken);
        public StringBuilder GetNodeBuilder();
    }
}