namespace MarkDown
{
    public class HeaderToken : Token
    {
        public HeaderToken(int start, int length) : base(start, length)
        {
        }

        public HeaderToken(int start) : base(start)
        {
        }

        public override bool IsFullLiner => true;
        public override string OpenedHtmlTag => "<h1>";
        public override string ClosedHtmlTag => "</h1>";
        public override int RawLengthOpen => 2;
        public override int RawLengthClose => 0;
        public override bool CanBeOpened(string text, int i)
        {
            return i == 0 && text[i] == '#' && TextHelper.CheckIfIthIsSpecificChar(text, i + 1, ' ');
        }

        public override bool CanBeClosed(string text, int i)
        {
            return false;
        }

        public override HeaderToken CreateNewTokenOfSameType(int start)
        {
            return new HeaderToken(start);
        }
    }
}
