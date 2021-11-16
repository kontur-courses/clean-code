using System;

namespace Markdown
{
    internal class BoldToken : StyleToken
    {
        public BoldToken(int openIndex) : base(openIndex) { }
        public new static string Separator => "__";

        internal override void Accept(MdParser parser)
        {
            parser.Handle(this);
        }
    }
}