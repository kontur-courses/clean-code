using System.Collections.Generic;

namespace Markdown
{
    public class Document
    {
        public TreeNode RootNode { get; }

        public Document()
        {
            RootNode = new TreeNode()
            {
                IsRoot = true,
                Child = new List<TreeNode>()
            };
        }
    }
}