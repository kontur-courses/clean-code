using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Markdown
{
    public class MarkdownTree
    {
        public readonly List<MarkdownTree> Children;
        public Tag NodeTag { get; }
        public string Content { get; }

        public MarkdownTree(Tag nodeTag, string content)
        {
            NodeTag = nodeTag;
            Content = content;
            Children = new List<MarkdownTree>();
        }

        public IReadOnlyList<MarkdownTree> GetChildren()
        {
            return Children;
        }

        public void AddChildren(IEnumerable<MarkdownTree> childrenToAdd)
        {
            foreach (var child in childrenToAdd)
                Children.Add(child);
        }

        public void AddChild(MarkdownTree child)
        {
            Children.Add(child);
        }
    }
}
