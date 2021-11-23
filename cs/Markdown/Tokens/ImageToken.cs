using System;
using Markdown.Parser;

namespace Markdown.Tokens
{
    public class ImageToken : Token
    {
        public const string Separator = "![";

        private string altText;
        private string source;
        public override bool IsNonPaired => true;
        public override bool IsContented => true;
        public ImageToken(int openIndex) : base(openIndex) { }
        internal ImageToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }
        internal ImageToken(int openIndex, int closeIndex, string source, string altText) : base(openIndex, closeIndex)
        {
            this.source = source;
            this.altText = altText;
        }

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

        public override string GetSeparator()
        {
            return Separator;
        }

        internal override bool Validate(IMdParser parser)
        {
            var text = parser.TextToParse;
            var endOfAltText = text.IndexOf(']', OpenIndex);
            var startOfSource = text.IndexOf('(', OpenIndex);
            var endOfSource = text.IndexOf(')', OpenIndex);
            var endOfParagraph = text.IndexOf('\n', OpenIndex);
            if (endOfParagraph < 0)
                endOfParagraph = text.Length - 1;

            if (endOfAltText == -1 || startOfSource == -1 || endOfSource == -1)
                return false;

            if (!(endOfAltText < startOfSource && startOfSource < endOfSource))
                return false;

            if (startOfSource != endOfAltText + 1 || endOfSource > endOfParagraph)
                return false;

            var altText = text.Substring(OpenIndex + GetSeparator().Length, endOfAltText - OpenIndex - GetSeparator().Length);
            AltText = altText;
            var source = text.Substring(startOfSource + 1, endOfSource - startOfSource - 1);
            Source = source;
            parser.AddScreening(new ScreeningToken(OpenIndex, endOfSource));

            Close(endOfSource);
            return true;
        }
    }
}