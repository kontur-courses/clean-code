using System.Linq;

namespace Markdown.Tree
{
    public class RootNode : Node
    {
        public override string StartWrapper => "";
        public override string EndWrapper => "";

        public override string GetText()
        {
            return string.Join("", Children.Select(c => c.GetText()));
        }
    }
}