using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Nodes
{
    public abstract class TaggedNode: JoiningNode
    {
        private readonly string htmlTag;
        private bool isClosed;
        
        public TaggedNode(string htmlTag)
        {
            this.htmlTag = htmlTag;
        }
        
        public void Close(Marking initialMarking, out Marking trimmedMarking)
        {
            if (isClosed)
                throw new Exception("Tag can not be closed twice");
            if (initialMarking == null)
                throw new ArgumentNullException(nameof(initialMarking));
            isClosed = true;
            trimmedMarking = TrimMarking(initialMarking);
        }

        public override StringBuilder GetNodeBuilder()
        {
            var builder = new StringBuilder();

            if (isClosed)
            {
                builder.Append(GetHtmlOpeningBracket());
                builder.Append(base.GetNodeBuilder());
                builder.Append(GetHtmlClosingBracket());
            }
            else
            {
                builder.Append(GetMarkdownOpening());
                builder.Append(base.GetNodeBuilder());
            }

            return builder;
        }

        public abstract bool ShouldBeClosed(Marking marking);

        public abstract Marking TrimMarking(Marking initialMarking);

        public abstract string GetMarkdownOpening();

        private string GetHtmlOpeningBracket()
        {
            return $"\\<{htmlTag}>";
        }
        
        private string GetHtmlClosingBracket()
        {
            return $"\\</{htmlTag}>";
        }
    }
}