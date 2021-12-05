using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class StringNode : INode
    {
        public NodeCondition Condition { get; protected set; }
        
        private StringBuilder builder;

        public StringNode(string value)
        {
            builder = new StringBuilder(value);
            Condition = NodeCondition.Opened;
        }

        public bool TryOpen(Stack<INode> openedNodes, List<IToken> tokens, ref int parentTokenPosition)
        {
            return false;
        }

        public void AddChild(INode child)
        {
            if (Condition == NodeCondition.Closed)
                throw new Exception("Closed node can not add anything to itself");
            builder.Append(child.GetNodeBuilder());
        }

        public void UpdateCondition(IToken newToken)
        {
            return;
        }

        public StringBuilder GetNodeBuilder()
        {
            return builder;
        }
    }
}