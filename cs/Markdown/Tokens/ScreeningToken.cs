using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class ScreeningToken : NonPairedToken
    {
        public ScreeningToken(int openIndex) : base(openIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Handle(this);
        }
    }
}