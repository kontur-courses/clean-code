using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class ScreeningToken : NonPairedToken
    {
        public ScreeningToken(int openIndex) : base(openIndex) { }
        internal ScreeningToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }
    }
}