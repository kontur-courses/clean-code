using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownDocument
    {
        public TreeNode RootNode { get; }

        public MarkdownDocument()
        {
            RootNode = new TreeNode()
            {
                IsRoot = true,
                Child = new List<TreeNode>()
            };
        }
    }
    
    public enum NodeType
    {
        SingleUnderlineTag,
        DoubleUnderlineTag,
        Text,
    }
    
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

        public TreeNode AppendNode(NodeType type)
        {
            var node = new TreeNode() { Type = type };
            Child.Add(node);
            return node;
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

    public class TextNode : TreeNode
    {
        public string Text { get; }

        public TextNode(string text)
        {
            Type = NodeType.Text;
            Text = text;
        }
    }
}