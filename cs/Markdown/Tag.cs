namespace Markdown
{
    public class Tag
    {
        public Tag(int position, TagType type, bool isOpening, int mdTagLength, bool inWord, bool isPaired)
        {
            Position = position;
            Type = type;
            IsOpening = isOpening;
            MdTagLength = mdTagLength;
            InWord = inWord;
            IsPaired = isPaired;
        }


        public int Position { get; }
        public TagType Type { get; }
        public bool IsOpening { get; private set; }
        public int MdTagLength { get; }

        public bool InWord { get; private set; }

        public bool IsPaired { get; }

        public void ConvertToClose()
        {
            IsOpening = false;
        }

        public void UnpinFromWord()
        {
            InWord = false;
        }
    }
}