using System;
using Markdown.Parser;

namespace Markdown.Tokens
{
    public class ImageToken : Token
    {
        public static string Separator = "![";
        private string altText;
        private string source;
        public override bool IsNonPaired => true;
        public override bool IsContented => true;

        public override string Source
        {
            get => source ?? string.Empty;
            set
            {
                if (source == null)
                    source = value;
                else
                    throw new InvalidOperationException("There is already a source");
            }
        }

        public override string AltText
        {
            get => altText ?? string.Empty;
            set
            {
                if (altText == null)
                    altText = value;
                else
                    throw new InvalidOperationException("There is already an alt text");
            }
        }

        public ImageToken(int openIndex) : base(openIndex) { }
        public ImageToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal ImageToken(int openIndex, int closeIndex, string source, string altText) : base(openIndex, closeIndex)
        {
            this.source = source;
            this.altText = altText;
        }

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