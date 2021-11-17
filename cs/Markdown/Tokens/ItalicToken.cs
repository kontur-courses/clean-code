using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class ItalicToken : StyleToken
    {
        public ItalicToken(int openIndex) : base(openIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Handle(this);
        }
    }
}