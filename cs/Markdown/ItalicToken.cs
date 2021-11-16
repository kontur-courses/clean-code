namespace Markdown
{
    internal class ItalicToken : StyleToken
    {
        public ItalicToken(int openIndex) : base(openIndex) { }

        public new static string Separator => "_";

        internal override void Accept(MdParser parser)
        {
            parser.Handle(this);
        }
    }
}