using System;
using System.Linq;

namespace Markdown.Tree
{
    public class ItalicNode : Node
    {
        public override string StartWrapper => "<em>";
        public override string EndWrapper => "</em>";

        public override string GetText()
        {
            var childrenText = string.Join("", Children.Select(c => c.GetText()));

            return StartWrapper + childrenText + EndWrapper;
        }
    }
}