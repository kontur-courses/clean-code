using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class ItalicsTag : MarkdownTag, ITag
    {
        public override string OpeningMarkup => "_";
        public override string ClosingMarkup => "_";
        public override string OpeningTag => "<em>";
        public override string ClosingTag => "</em>";

        public new void Replace(List<string> builder, int start, int end)
        {
            builder[start] = OpeningTag;
            builder[end] = ClosingTag;
        }
    }
}