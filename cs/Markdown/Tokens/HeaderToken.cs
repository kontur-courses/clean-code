using Markdown.Parser;

namespace Markdown.Tokens
{
    public class HeaderToken : Token
    {
        public const string Separator = "# ";

        public override bool IsNonPaired => true;
        public override bool IsContented => false;
        public HeaderToken(int openIndex) : base(openIndex) { }
        internal HeaderToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        public override string GetSeparator()
        {
            return Separator;
        }

        internal override bool Validate(IMdParser parser)
        {
            if (OpenIndex != 0 && parser.TextToParse[OpenIndex - 1] != '\n' && parser.TextToParse[OpenIndex - 1] != '\r')
                return false;

            var closeIndexLf = parser.TextToParse.IndexOf('\n', OpenIndex);

            var closeIndex = closeIndexLf > 0 && parser.TextToParse[closeIndexLf - 1] == '\r'
                ? closeIndexLf - 1
                : closeIndexLf;

            if (closeIndex == -1)
                closeIndex = parser.TextToParse.Length;

            Close(closeIndex);

            return true;
        }
    }
}