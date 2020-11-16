using System.Collections.Generic;

namespace Markdown
{
    public class MdEmphasisStyleFinder : MdStyleFinder
    {
        private readonly Stack<int> endTagsPositions = new Stack<int>();
        private readonly Stack<int> startTagsPositions = new Stack<int>();

        public MdEmphasisStyleFinder(Style mdStyle, TextInfo textInfo) : base(mdStyle, textInfo)
        {
        }

        protected override (int Start, int End) GetNextTagPairPositions()
        {
            while (true)
            {
                if (startTagsPositions.Count == 0)
                    if (!FindStartTag())
                        return (-1, -1);
                if (endTagsPositions.Count == 0)
                    if (!FindEndTag())
                        return (-1, -1);
                var startTagPosition = startTagsPositions.Peek();
                var endTagPosition = endTagsPositions.Peek();
                if (IsTagPair(startTagPosition, endTagPosition))
                    return (startTagsPositions.Pop(), endTagsPositions.Pop());
                var startWord = Text.GetWordContainingCurrentSymbol(startTagPosition);
                if (startWord.ContainsInside(MdStyle.StartTag, startTagPosition))
                    startTagsPositions.Pop();
                else if (IsStartTag(endTagPosition))
                    startTagsPositions.Push(endTagsPositions.Pop());
            }
        }

        protected bool FindStartTag()
        {
            endTagsPositions.Clear();
            var startTag = GetNextStartTagPosition();
            if (startTag == -1)
                return false;
            startTagsPositions.Push(startTag);
            return true;
        }

        protected bool FindEndTag()
        {
            while (true)
            {
                var nextTag = GetNextTagPosition(MdStyle.EndTag);
                if (nextTag == -1)
                    return false;
                if (IsEndTag(nextTag))
                {
                    endTagsPositions.Push(nextTag);
                    return true;
                }

                if (IsStartTag(nextTag))
                    startTagsPositions.Push(nextTag);
            }
        }

        protected int GetNextTagPosition(string tag)
        {
            while (true)
            {
                var tagPosition = Text.IndexOf(tag, Pointer);
                if (IsEscaped(tagPosition))
                {
                    Pointer++;
                    continue;
                }

                if (tagPosition == -1)
                    Pointer = Text.Length;
                else
                    Pointer = tagPosition + tag.Length;
                return tagPosition;
            }
        }

        protected int GetNextStartTagPosition()
        {
            var startTagPosition = GetNextTagPosition(MdStyle.StartTag);
            while (startTagPosition != -1 && !IsStartTag(startTagPosition))
                startTagPosition = GetNextTagPosition(MdStyle.StartTag);
            return startTagPosition;
        }

        protected bool IsTagPair(int startTagPosition, int endTagPosition)
        {
            var startWord = Text.GetWordContainingCurrentSymbol(startTagPosition);
            var endWord = Text.GetWordContainingCurrentSymbol(endTagPosition);
            return !(!startWord.Equals(endWord)
                     && (startWord.ContainsInside(MdStyle.StartTag, startTagPosition)
                         || endWord.ContainsInside(MdStyle.EndTag, endTagPosition)))
                   && !IsEmptyStringInside(startTagPosition, endTagPosition);
        }

        public virtual bool IsStartTag(int startTagPosition)
        {
            var word = Text.GetWordContainingCurrentSymbol(startTagPosition);
            return !(word.ContainsInside(MdStyle.StartTag, startTagPosition) && word.ContainsDigit())
                   && !(startTagPosition + MdStyle.StartTag.Length < Text.Length
                        && char.IsWhiteSpace(Text[startTagPosition + MdStyle.StartTag.Length]));
        }

        public virtual bool IsEndTag(int endTagPosition)
        {
            var word = Text.GetWordContainingCurrentSymbol(endTagPosition);
            return !(word.ContainsInside(MdStyle.StartTag, endTagPosition) && word.ContainsDigit())
                   && !(endTagPosition == 0 || char.IsWhiteSpace(Text[endTagPosition - 1]));
        }
    }
}