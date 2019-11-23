using System;

namespace MarkDown.TagParsers
{
    public class Tag
    {
        public readonly bool IsOpening;
        private readonly int length;
        public readonly int StartIndex;
        public TagType Type;

        public Tag(int startIndex, int length, TagType type, bool isOpening = true)
        {
            if (length < 0) throw new ArgumentException("Length can't be less than zero");
            if (startIndex < 0) throw new ArgumentException("StartIndex can't be less than zero");
            StartIndex = startIndex;
            this.length = length;
            Type = type;
            IsOpening = isOpening;
        }

        public int IndexNextToTag => StartIndex + length;
    }
}