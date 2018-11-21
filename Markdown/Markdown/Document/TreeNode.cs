using System.Collections.Generic;

namespace Markdown
{
    public class TreeNode
    {
        public bool IsRoot;
        public NodeType Type;
        public List<TreeNode> Child;

        public TreeNode()
        {
            IsRoot = false;
            Child = new List<TreeNode>();
        }

        public TreeNode AppendNode(TreeNode node)
        {
            Child.Add(node);
            return node;
        }

        public TreeNode AppendTextNode(string text)
        {
            var textNode = new TextNode(text);
            Child.Add(textNode);
            return textNode;
        }
    }
}