using Markdown.Parser;

namespace Markdown.Tokens
{
    public class ScreeningToken : Token
    {
        public const string Separator = "\\";

        public override bool IsNonPaired => true;
        public override bool IsContented => false;

        public ScreeningToken(int openIndex) : base(openIndex) { }
        internal ScreeningToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        public override string GetSeparator()
        {
            return Separator;
        }

        internal override bool Validate(IMdParser parser)
        {
            Close(OpenIndex);
            parser.AddScreening(this);

            return false;
        }
    }
}