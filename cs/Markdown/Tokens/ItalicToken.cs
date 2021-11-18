using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class ItalicToken : StyleToken
    {
        public ItalicToken(int openIndex) : base(openIndex) { }
        internal ItalicToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override void Accept(MdParser parser)
        {
            parser.Visit(this);
        }
    }
}