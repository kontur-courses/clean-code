using System;

namespace Markdown
{
    public class TagToken : IComparable<TagToken>
    {
        public readonly int StartPosition;
        public readonly string Tag;

        public TagToken(int startPosition, string tag)
        {
            StartPosition = startPosition;
            Tag = tag;
        }

        public bool? IsOpening { get; private set; }

        public int CompareTo(TagToken other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var startPositionComparison = StartPosition.CompareTo(other.StartPosition);
            if (startPositionComparison != 0) return startPositionComparison;
            return string.Compare(Tag, other.Tag, StringComparison.Ordinal);
        }

        public void InitIsOpening(bool isOpening)
        {
            if (IsOpening is null)
                IsOpening = isOpening;
            else
                throw new Exception("use only for initialisation");
        }


        protected bool Equals(TagToken other)
        {
            return StartPosition == other.StartPosition && Tag == other.Tag;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartPosition, Tag);
        }
    }
}