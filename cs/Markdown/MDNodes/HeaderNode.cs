using Markdown.Abstractions;

namespace Markdown.MDNodes
{
    public class HeaderNode : BlockNode
    {
        private const string OpeningTag = "<h1>";
        private const string ClosingTag = "</h1>";

        public override string Render()
        {
            var inner = string.Join("", Children.Select(c => c.Render()));
            return $"{OpeningTag}{inner}{ClosingTag}";
        }
    }
}
