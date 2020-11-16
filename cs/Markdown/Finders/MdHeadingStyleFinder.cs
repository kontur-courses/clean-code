namespace Markdown
{
    public class MdHeadingStyleFinder : MdStyleFinder
    {
        public MdHeadingStyleFinder(Style mdStyle, TextInfo textInfo) : base(mdStyle, textInfo)
        {
        }

        protected override (int Start, int End) GetNextTagPairPositions()
        {
            while (true)
            {
                var startTagPosition = GetNextStartTagPosition();
                Pointer = startTagPosition + MdStyle.StartTag.Length;
                var endTagPosition = GetNextEndTagPosition();
                return (startTagPosition, endTagPosition);
            }
        }

        protected int GetNextEndTagPosition()
        {
            return Text.GetEndOfParagraphPosition(Pointer);
        }

        protected int GetNextStartTagPosition()
        {
            int startTagPosition;
            while ((startTagPosition = Text.IndexOf(MdStyle.StartTag, Pointer)) != -1)
            {
                if (IsStartTag(startTagPosition))
                    return startTagPosition;
                Pointer = startTagPosition + MdStyle.StartTag.Length;
            }

            return startTagPosition;
        }

        private bool IsStartTag(int startTagPosition)
        {
            return Text.IsStartOfParagraph(startTagPosition) && !IsEscaped(startTagPosition);
        }
    }
}