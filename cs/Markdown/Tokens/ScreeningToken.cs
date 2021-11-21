using Markdown.Parser;

namespace Markdown.Tokens
{
    public class ScreeningToken : Token
    {
        public static readonly string Separator = "\\";

        public override bool IsNonPaired => true;
        public override bool IsContented => false;
        public override string GetSeparator() => Separator;

        public ScreeningToken(int openIndex) : base(openIndex) { }
        internal ScreeningToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }
    }
}