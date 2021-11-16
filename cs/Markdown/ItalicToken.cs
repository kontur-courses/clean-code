namespace Markdown
{
    internal class ItalicToken : StyleToken
    {
        public ItalicToken(int openIndex) : base(openIndex) { }

        public new static string Separator => "_";

        internal override void Handle(MdParser parser)
        {
            throw new System.NotImplementedException();
        }
    }
}