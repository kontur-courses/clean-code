using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public abstract class MdStyleFinder
    {
        public readonly Style MdStyle;
        public readonly string Text;
        protected int pointer;

        public MdStyleFinder(Style mdStyle, string text)
        {
            MdStyle = mdStyle;
            Text = text;
        }

        public IEnumerable<Element> Find()
        {
            pointer = 0;
            while (true)
            {
                var tagPairPositions = GetNextTagPairPositions();
                if (tagPairPositions == null)
                    break;
                pointer = tagPairPositions.Item2 + MdStyle.EndTag.Length;
                yield return Element.Create(MdStyle, tagPairPositions.Item1, tagPairPositions.Item2);
            }
            pointer = 0;
        }
        protected Tuple<int, int> GetNextTagPairPositions()
        {
            while (true)
            {
                var startTagPosition = GetNextStartTagPosition();
                pointer = startTagPosition + MdStyle.StartTag.Length;
                var endTagPosition = GetNextEndTagPosition();
                if (startTagPosition == -1 || endTagPosition == -1)
                    break;
                if (IsTagPair(startTagPosition, endTagPosition))
                    return Tuple.Create(startTagPosition, endTagPosition);
            }
            return null;
        }

        protected bool IsEmptyStringInside(int startTagPosition, int endTagPosition)
        {
            return endTagPosition - startTagPosition - MdStyle.StartTag.Length == 0;
        }

        protected abstract int GetNextStartTagPosition();

        protected abstract int GetNextEndTagPosition();

        protected abstract bool IsTagPair(int startTagPosition, int endTagPosition);
    }
}
