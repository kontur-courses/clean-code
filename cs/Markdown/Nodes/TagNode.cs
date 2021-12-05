using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public abstract class TaggedNode: INode
    {
        public NodeCondition Condition { get; set; }

        private readonly List<INode> children = new List<INode>();
        private readonly string htmlTag;
        private readonly string markdownTag;
        
        protected TaggedNode(string htmlTag, string markdownTag)
        {
            this.htmlTag = htmlTag;
            this.markdownTag = markdownTag;
        }

        public virtual void AddChild(INode child)
        {
            children.Add(child);
        }
        
        public abstract bool TryOpen(Stack<INode> openedNodes, List<IToken> tokens, ref int parentTokenPosition);
        public abstract void UpdateCondition(IToken newToken);

        public StringBuilder GetNodeBuilder()
        {
            var builder = new StringBuilder();

            if (Condition == NodeCondition.Closed)
            {
                builder.Append(GetHtmlOpeningBracket());
                builder.AppendJoin("", children.Select(x => x.GetNodeBuilder()));
                builder.Append(GetHtmlClosingBracket());
            }
            else
            {
                builder.Append(markdownTag);
                builder.AppendJoin("", children.Select(x => x.GetNodeBuilder()));
            }

            return builder;
        }
        
        private string GetHtmlOpeningBracket()
        {
            return $"<{htmlTag}>";
        }
        
        private string GetHtmlClosingBracket()
        {
            return $"</{htmlTag}>";
        }
    }
}