using Markdown.Parser;

namespace Markdown.Tokens
{
    public class HeaderToken : Token
    {
        public static readonly string Separator = "# ";

        public override bool IsNonPaired => true;
        public override bool IsContented => false;
        public override string GetSeparator() => Separator;
        public HeaderToken(int openIndex) : base(openIndex) { }
        internal HeaderToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }
    }
}