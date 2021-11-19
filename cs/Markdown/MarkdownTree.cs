using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Markdown
{
    public class MarkdownTree
    {
        private readonly List<MarkdownTree> _children;
        public Tag NodeTag { get; }

        public MarkdownTree(Tag nodeTag)
        {
            NodeTag = nodeTag;
            _children = new List<MarkdownTree>();
        }

        public List<MarkdownTree> GetChildren()
        {
            return _children.ToList();
        }

        public void AddChild(MarkdownTree child)
        {
            _children.Add(child);
        }
    }
}
