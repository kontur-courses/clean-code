namespace Markdown
{
    public class Mark
    {
        public static readonly Mark RawMark = new Mark("", "", "");

        public readonly string Sign;

        public readonly string OpeningTag;

        public readonly string ClosingTag;

        public readonly int Length;

        public Mark(string sign, string openingTag, string closingTag)
        {
            Sign = sign;
            OpeningTag = openingTag;
            ClosingTag = closingTag;
            Length = sign.Length;
        }

        public bool Fits(Mark mark)
        {
            return Sign == mark.Sign ;
        }
    }
}
