using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public abstract class TaggedNode: INode
    {
        public bool IsClosed { get; private set; }
        
        private readonly List<INode> children = new List<INode>();
        private readonly string htmlTag;
        private readonly string markdownTag;

        public virtual void AddChild(INode child)
        {
            children.Add(child);
        }
        
        public abstract bool TryOpen(List<IToken> tokens, ref int parentTokenPosition);
        public abstract bool ShouldBeClosedByNewToken(List<IToken> tokens, int anotherTokenPosition);
        public abstract bool CannotBeClosed(List<IToken> tokens, int anotherTokenPosition);
        public abstract bool ShouldBeClosedWhenParagraphEnds();

        protected TaggedNode(string htmlTag, string markdownTag)
        {
            this.htmlTag = htmlTag;
            this.markdownTag = markdownTag;
        }

        public void Close()
        {
            if (IsClosed)
                throw new Exception("Tag can not be closed twice");
            IsClosed = true;
        }

        public StringBuilder GetNodeBuilder()
        {
            var builder = new StringBuilder();

            if (IsClosed)
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