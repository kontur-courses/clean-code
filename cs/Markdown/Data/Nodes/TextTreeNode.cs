namespace Markdown.Data.Nodes
{
    public class TextTreeNode : TokenTreeNode
    {
        public readonly string Text;

        public TextTreeNode(string text)
        {
            Text = text;
        }
    }
}