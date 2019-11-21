using System.Collections.Generic;
using System.Text;

namespace MarkDown.TagParsers
{
    public class StrongTag : Tag
    {
        public override string OpeningHtmlTag { get; } = "<strong>";
        public override string ClosingHtmlTag { get; } = @"</strong>";
        public override string MdTag { get; } = "__";
    }
}