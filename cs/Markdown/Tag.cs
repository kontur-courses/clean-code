using System;

namespace Markdown
{
    public class Tag
    {
        public bool CanBeClose;
        public bool CanBeOpen;
        public bool InWord;
        public TagStatus Status;
        public Word Word;

        public Tag(TagType type, int position, Word word, bool canBeOpen, bool canBeClose)
        {
            Position = position;
            Word = word;
            CanBeOpen = canBeOpen;
            CanBeClose = canBeClose;
            Type = type;
        }

        public TagType Type { get; }
        public int Position { get; }

        public string GetHtmlTag()
        {
            if (Status == TagStatus.Open)
                return Type.GetOpenHtmlTag();
            if (Status == TagStatus.Close)
                return Type.GetCloseHtmlTag();
            throw new ArgumentException();
        }
    }
}