using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class HeaderToken : NonPairedToken
    {
        public HeaderToken(int openIndex) : base(openIndex) { }
        internal HeaderToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }
    }
}