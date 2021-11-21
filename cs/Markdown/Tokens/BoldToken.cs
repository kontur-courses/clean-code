using Markdown.Parser;

namespace Markdown.Tokens
{
    public class BoldToken : Token
    {
        public static readonly string Separator = "__";

        public override bool IsNonPaired => false;
        public override bool IsContented => false;
        public BoldToken(int openIndex) : base(openIndex) { }

        internal BoldToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

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