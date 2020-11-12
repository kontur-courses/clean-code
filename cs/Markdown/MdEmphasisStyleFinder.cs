using System;
using System.Collections.Generic;
using System.Text;

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
            while ((endTagPosition = Text.IndexOf(MdStyle.EndTag, pointer)) != -1)
            {
                if (IsEndTag(endTagPosition))
                    return endTagPosition;
                pointer = endTagPosition + MdStyle.EndTag.Length;
            }
            return endTagPosition;
        }

        protected override int GetNextStartTagPosition()
        {
            int startTagPosition;
            while ((startTagPosition = Text.IndexOf(MdStyle.StartTag, pointer)) != -1)
            {
                if (IsStartTag(startTagPosition))
                    return startTagPosition;
                pointer = startTagPosition + MdStyle.StartTag.Length;
            }
            return startTagPosition;
        }

        protected override bool IsTagPair(int startTagPosition, int endTagPosition)
        {
            var startWord = Text.GetWordContainingCurrentSymbol(startTagPosition);
            var endWord = Text.GetWordContainingCurrentSymbol(endTagPosition);
            if (!startWord.Equals(endWord))
            {
                if (startWord.IsInside(MdStyle.StartTag, startTagPosition)
                    || endWord.IsInside(MdStyle.EndTag, endTagPosition))
                    return false;
            }
            if (IsEmptyStringInside(startTagPosition, endTagPosition))
            {
                return false;
            }
            return true;
        }

        public virtual bool IsStartTag(int startTagPosition)
        {
            var word = Text.GetWordContainingCurrentSymbol(startTagPosition);
            if (word.IsInside(MdStyle.StartTag, startTagPosition) && word.ContainsDigit())
                return false;
            if (startTagPosition + MdStyle.StartTag.Length < Text.Length
                && Char.IsWhiteSpace(Text[startTagPosition + MdStyle.StartTag.Length]))
                return false;
            return true;
        }

        public virtual bool IsEndTag(int endTagPosition)
        {
            var word = Text.GetWordContainingCurrentSymbol(endTagPosition);
            if (word.IsInside(MdStyle.StartTag, endTagPosition) && word.ContainsDigit())
                return false;
            if (endTagPosition == 0 || Char.IsWhiteSpace(Text[endTagPosition - 1]))
                return false;
            return true;
        }
    }
}
