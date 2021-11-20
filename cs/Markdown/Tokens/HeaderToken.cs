using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class HeaderToken : Token
    {
        public static readonly string Separator = "#";

        public override bool IsNonPaired => true;
        public HeaderToken(int openIndex) : base(openIndex) { }
        internal HeaderToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }

        public override string GetSeparator()
        {
            return Separator;
        }
    }
}