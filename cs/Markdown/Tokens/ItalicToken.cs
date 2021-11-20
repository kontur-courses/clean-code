using Markdown.Parser;

namespace Markdown.Tokens
{
    internal class ItalicToken : Token
    {
        public static readonly string Separator = "_";

        public override bool IsNonPaired => false;
        public ItalicToken(int openIndex) : base(openIndex) { }
        internal ItalicToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

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