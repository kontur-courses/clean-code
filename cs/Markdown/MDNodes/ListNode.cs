using Markdown.Abstractions;

namespace Markdown.MDNodes
{
    public class ListNode : BlockNode
    {
        private const string OpeningTag = "<ul>";
        private const string ClosingTag = "</ul>";

        public List<ListItemNode> Items { get; } = new List<ListItemNode>();

        public override string Render() =>
            $"{OpeningTag}{string.Concat(Items.Select(item => item.Render()))}{ClosingTag}";
    }
}