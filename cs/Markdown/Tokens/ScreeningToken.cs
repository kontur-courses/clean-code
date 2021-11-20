using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class ScreeningToken : Token
    {
        public static readonly string Separator = "\\";

        public override bool IsNonPaired => true;
        public ScreeningToken(int openIndex) : base(openIndex) { }
        internal ScreeningToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        public override string GetSeparator()
        {
            return Separator;
        }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }
    }
}