using System;
using System.Linq;

namespace Markdown
{
    public abstract class WordTag : Tag
    {
        protected WordTag(string mdTag, string htmlTag, int position, bool isPaired,
            bool isOpening)
            : base(mdTag, htmlTag, position, isOpening)
        {
        }

        public bool InWord(string text)
        {
            return Position != 0
                   && Position + MdTag.Length != text.Length
                   && !char.IsWhiteSpace(text[Position - 1])
                   && !char.IsWhiteSpace(text[Position + MdTag.Length]);
        }

        public bool CanBeOpen(string text)
        {
            var positionAfterTag = Position + MdTag.Length;
            return positionAfterTag < text.Length && !char.IsWhiteSpace(text[positionAfterTag]);
        }

        private bool CanBeClose(string text)
        {
            var positionBeforeTag = Position - 1;
            return Position > 0 && !char.IsWhiteSpace(text[positionBeforeTag]);
        }

        public bool TryPairCloseTag(WordTag closeTag, string text)
        {
            if (!closeTag.CanBeClose(text) || !InOneParagraphWith(closeTag, text))
                return false;
            var anyTagInWord = InWord(text) || closeTag.InWord(text);
            if (anyTagInWord)
            {
                if (!InOneWordWith(closeTag, text))
                    return false;
                closeTag.IsOpening = false;
                return true;
            }

            closeTag.IsOpening = false;
            return true;
        }

        public bool InOneWordWith(WordTag otherTag, string text)
        {
            return !text.Substring(Position, otherTag.Position - Position).Any(char.IsWhiteSpace);
        }
        
        public bool InOneParagraphWith(WordTag otherTag, string text)
        {
            return !text.Substring(Position, otherTag.Position - Position).Contains(Environment.NewLine);
        }
    }
}