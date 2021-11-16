using System;

namespace Markdown
{
    internal class BoldToken : StyleToken
    {
        public BoldToken(int openIndex) : base(openIndex) { }
        public new static string Separator => "__";

        internal override void Handle(MdParser parser)
        {
            if (!parser.Tokens.ContainsKey(GetType()))
                parser.Tokens.Add(GetType(), this);

            var newLineIndex = parser.TextToParse.IndexOf('\n');
            var newBoldIndex = parser.TextToParse.IndexOf("__");
            var newItalicIndex = parser.TextToParse.IndexOf('_');

            throw new System.NotImplementedException();
        }
    }
}