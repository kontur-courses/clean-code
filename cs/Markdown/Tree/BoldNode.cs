using System;
using System.Linq;

namespace Markdown.Tree
{
    public class BoldNode : Node
    {
        public override string StartWrapper => "<strong>";
        public override string EndWrapper => "</strong>";

        public override string GetText()
        {
            var childrenText = string.Join("", Children.Select(c => c.GetText()));

            if (Parent is ItalicNode)
                return childrenText;

            return StartWrapper + childrenText + EndWrapper;
        }
    }
}