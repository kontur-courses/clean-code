using System.Collections.Generic;

namespace Markdown.Tree
{
    public abstract class Node
    {
        public abstract string StartWrapper { get; }
        public abstract string EndWrapper { get; }

        protected Node()
        {
            Children = new List<Node>();
        }

        public void AddNode(Node child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public List<Node> Children { get; }
        public Node Parent { get; set; }

        public abstract string GetText();
    }
}