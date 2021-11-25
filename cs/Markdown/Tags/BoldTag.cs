using System.Collections.Generic;

namespace Markdown
{
    public class BoldTag : MarkdownTag, ITag
    {
        public override string OpeningMarkup => "__";
        public override string ClosingMarkup => "__";
        public override string OpeningTag => "<strong>";
        public override string ClosingTag => "</strong>";

        public new void Replace(List<string> builder, int start, int end)
        {
            builder[start] = OpeningTag;
            builder[start + 1] = "";
            builder[end] = ClosingTag;
            builder[end - 1] = "";
        }
    }
}