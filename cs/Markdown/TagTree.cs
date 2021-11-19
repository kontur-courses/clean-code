using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Markdown
{
    public class TagTree
    {
        private readonly IEnumerable<TagTree> children;
        public Tag NodeTag { get; }

        public TagTree(Tag nodeTag)
        {
            NodeTag = nodeTag;
        }

        public List<TagTree> GetChildren()
        {
            return children.ToList();
        }
    }
}
