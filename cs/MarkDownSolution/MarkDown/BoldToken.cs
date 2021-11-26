namespace MarkDown
{
    public class BoldToken : Token
    {
        public BoldToken(int start) : base(start)
        {
        }

        public BoldToken(int start, int length) : base(start, length)
        {
        }

        public override string OpenedHtmlTag => "<strong>";

        public override string ClosedHtmlTag => "</strong>";

        public override int RawLengthOpen => 2;

        public override int RawLengthClose => 2;

        public override bool CanBeClosed(string text, int i)
        {
            return !TextHelper.CheckIfIthIsSpecificChar(text, i - 2, ' ')
                && TextHelper.CheckIfIthIsSpecificChar(text, i - 1, '_')
                && TextHelper.CheckIfIthIsSpecificChar(text, i, '_');

        }

        public override bool CanBeOpened(string text, int i)
        {
            return TextHelper.CheckIfIthIsSpecificChar(text, i + 1, '_')
                && !TextHelper.CheckIfIthIsSpecificChar(text, i + 2, ' ')
                && !TextHelper.IsThreeGroundsInRow(text, i)
                && TextHelper.CheckIfIthIsSpecificChar(text, i, '_');
        }

        public override BoldToken CreateNewTokenOfSameType(int start)
        {
            return new BoldToken(start);
        }
    }
}
