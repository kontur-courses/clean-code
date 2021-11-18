using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class BoldToken : StyleToken
    {
        public BoldToken(int openIndex) : base(openIndex) { }

        internal BoldToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }
    }
}