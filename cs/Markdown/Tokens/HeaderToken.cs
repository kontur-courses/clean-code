using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class HeaderToken : NonPairedToken

    {
        public HeaderToken(int openIndex) : base(openIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Handle(this);
        }
    }
}