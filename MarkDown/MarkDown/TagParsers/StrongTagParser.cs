using System.Text;

namespace MarkDown.TagParsers
{
    public class StrongTagParser : TagParser
    {
        public override string OpeningHtmlTag { get; } = "<strong>";
        public override string ClosingHtmlTag { get; } = @"<strong\>";
        public override string MdTag { get; } = "__";

        public override StringBuilder ParseLine(StringBuilder line)
        {
            throw new System.NotImplementedException();
        }
    }
}