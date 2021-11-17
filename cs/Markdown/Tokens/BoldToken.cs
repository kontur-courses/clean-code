using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class BoldToken : StyleToken
    {
        public BoldToken(int openIndex) : base(openIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Handle(this);
        }
    }
}