namespace Markdown
{
    public class TagInfo : IComparable
    {
        public int position;
        public TagType type;
        public bool canBeStarter;
        public bool canBeEnder;
        public bool IsEscaped;
        public bool InPair { get; set; }

        public TagInfo()
        {
        }

        public TagInfo(int pos, TagType type, bool canBeStarter = false, bool canBeEnder = false, bool isEscaped = false)
        {
            position = pos;
            this.type = type;
            this.canBeStarter = canBeStarter;
            this.canBeEnder = canBeEnder;
            IsEscaped = isEscaped;
            InPair = false;
        }

        public int CompareTo(object? obj)
        {
            throw new NotImplementedException();

        }

        public bool InMiddle() => canBeStarter && canBeEnder;

    }
}
