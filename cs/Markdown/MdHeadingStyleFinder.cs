namespace Markdown
{
    public class MdHeadingStyleFinder : MdStyleFinder
    {
        public MdHeadingStyleFinder(Style mdStyle, string text) : base(mdStyle, text)
        {
        }

        protected override int GetNextEndTagPosition()
        {
            return Text.GetEndOfParagraphPosition(Pointer);
        }

        protected override int GetNextStartTagPosition()
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

        protected override bool IsTagPair(int startTagPosition, int endTagPosition)
        {
            return IsStartTag(startTagPosition);
        }

        private bool IsStartTag(int startTagPosition)
        {
            return Text.IsStartOfParagraph(startTagPosition);
        }
    }
}