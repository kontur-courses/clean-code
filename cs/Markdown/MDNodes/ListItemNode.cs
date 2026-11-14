using Markdown.Abstractions;

namespace Markdown.MDNodes
{
    public class ListItemNode : BlockNode
    {
        private const string OpeningTag = "<li>";
        private const string ClosingTag = "</li>";

        public ListItemNode(List<InlineNode> children)
        {
            Children = children;
        }

        public override string Render()
        {
            var inner = string.Join("", Children.Select(c => c.Render()));
            return $"{OpeningTag}{inner}{ClosingTag}";
        }
    }
}