namespace MarkDown
{
    public class ListElementToken : Token
    {
        public ListElementToken(int start) : base(start)
        {
        }

        public ListElementToken(int start, int length) : base(start, length)
        {
        }

        public override bool IsFullLiner => true;

        public override string OpenedHtmlTag => "<li>";

        public override string ClosedHtmlTag => "</li>";

        public override int RawLengthOpen => 2;

        public override int RawLengthClose => 0;

        public override bool CanBeClosed(string text, int i)
        {
            return false;
        }

        public override bool CanBeOpened(string text, int i)
        {
            return TextHelper.CheckIfIthIsSpecificChar(text, i, '*') 
                && i == 0
                && TextHelper.CheckIfIthIsSpecificChar(text, i + 1, ' ');
        }

        public override ListElementToken CreateNewTokenOfSameType(int start)
        {
            return new ListElementToken(start);
        }
    }
}
