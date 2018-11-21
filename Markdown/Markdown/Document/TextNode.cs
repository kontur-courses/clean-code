namespace Markdown
{
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