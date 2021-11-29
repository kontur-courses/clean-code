using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class StringNode : INode
    {
        private StringBuilder builder;
        private bool isClosed;

        public StringNode(string value)
        {
            this.builder = new StringBuilder(value);
        }

        public bool TryOpen(List<IToken> tokens, ref int parentTokenPosition)
        {
            return false;
        }

        public void AddChild(INode child)
        {
            if (isClosed)
                throw new Exception("Closed node can not add anything to itself");
            builder.Append(child.GetNodeBuilder());
        }

        public void Close()
        {
            if (isClosed)
                throw new Exception("Was already closed");
            isClosed = true;
        }

        public bool ShouldBeClosedByNewToken(List<IToken> tokens, int anotherTokenPosition)
        {
            return false;
        }

        public bool CannotBeClosed(List<IToken> tokens, int anotherTokenPosition)
        {
            return false;
        }

        public bool ShouldBeClosedWhenParagraphEnds()
        {
            return false;
        }

        public StringBuilder GetNodeBuilder()
        {
            return builder;
        }
    }
}