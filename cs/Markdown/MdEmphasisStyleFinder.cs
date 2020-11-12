namespace Markdown
{
    public class MdEmphasisStyleFinder : MdStyleFinder
    {
        public MdEmphasisStyleFinder(Style mdStyle, string text) : base(mdStyle, text)
        {
        }

        protected override int GetNextEndTagPosition()
        {
            int endTagPosition;
            while ((endTagPosition = Text.IndexOf(MdStyle.EndTag, Pointer)) != -1)
            {
                if (IsEndTag(endTagPosition))
                    return endTagPosition;
                Pointer = endTagPosition + MdStyle.EndTag.Length;
            }

            return endTagPosition;
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
            var startWord = Text.GetWordContainingCurrentSymbol(startTagPosition);
            var endWord = Text.GetWordContainingCurrentSymbol(endTagPosition);
            return !(!startWord.Equals(endWord) 
                     && (startWord.IsInside(MdStyle.StartTag, startTagPosition) 
                         || endWord.IsInside(MdStyle.EndTag, endTagPosition))) 
                   && !IsEmptyStringInside(startTagPosition, endTagPosition);
        }

        public virtual bool IsStartTag(int startTagPosition)
        {
            var word = Text.GetWordContainingCurrentSymbol(startTagPosition);
            return !(word.IsInside(MdStyle.StartTag, startTagPosition) && word.ContainsDigit())
                   && !(startTagPosition + MdStyle.StartTag.Length < Text.Length
                        && char.IsWhiteSpace(Text[startTagPosition + MdStyle.StartTag.Length]));
        }

        public virtual bool IsEndTag(int endTagPosition)
        {
            var word = Text.GetWordContainingCurrentSymbol(endTagPosition);
            return !(word.IsInside(MdStyle.StartTag, endTagPosition) && word.ContainsDigit())
                   && !(endTagPosition == 0 || char.IsWhiteSpace(Text[endTagPosition - 1]));
        }
    }
}