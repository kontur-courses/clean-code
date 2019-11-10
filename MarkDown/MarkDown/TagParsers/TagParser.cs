using System.Text;

namespace MarkDown.TagParsers
{
    public abstract class TagParser
    {
        public abstract string OpeningHtmlTag { get; }
        public abstract string ClosingHtmlTag { get; }
        public abstract string MdTag { get; }

        public abstract StringBuilder ParseLine(StringBuilder line);
    }
}