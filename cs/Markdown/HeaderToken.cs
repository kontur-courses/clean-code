namespace Markdown
{
    internal class HeaderToken : Token

    {
        public HeaderToken(int openIndex) : base(openIndex) { }

        public new static string Separator => "#";

        internal override void Accept(MdParser parser)
        {
            parser.Handle(this);
        }
    }
}