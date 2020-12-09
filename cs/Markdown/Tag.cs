namespace Markdown
{
    public class Tag
    {
        public Tag(int position, TagType type, bool isOpening, int mdTagLength, bool inWord, bool isMdPaired)
        {
            Position = position;
            Type = type;
            IsOpening = isOpening;
            MdTagLength = mdTagLength;
            InWord = inWord;
            IsMdPaired = isMdPaired;
        }


        public int Position { get; }
        public TagType Type { get; }
        public bool IsOpening { get; }
        public int MdTagLength { get; }
        public bool InWord { get; }
        public bool IsMdPaired { get; }

        public static Tag GetCloseTag(Tag tag)
        {
            return new Tag(tag.Position, tag.Type, false, tag.MdTagLength, tag.InWord, tag.IsMdPaired);
        }

        public static Tag GetNotInWordTag(Tag tag)
        {
            return new Tag(tag.Position, tag.Type, tag.IsOpening, tag.MdTagLength, false, tag.IsMdPaired);
        }
    }
}