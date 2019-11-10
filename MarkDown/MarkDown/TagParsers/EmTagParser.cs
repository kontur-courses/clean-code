using System.Text;

namespace MarkDown.TagParsers
{
    public class EmTagParser : TagParser
    {
        public override string OpeningHtmlTag { get; } = "<em>";
        public override string ClosingHtmlTag { get; } = @"<em\>";
        public override string MdTag { get; } = "_";
        public override StringBuilder ParseLine(StringBuilder line)
        {
            throw new System.NotImplementedException();
        }
    }
}