using System.Collections.Generic;
using Markdown.Tag;

namespace Markdown.Tree
{
    public class Node
    {
        public ITag Tag;
        public readonly List<Node> Children = new List<Node>();

        public Node(ITag tag)
        {
            Tag = tag;
        }
    }
}