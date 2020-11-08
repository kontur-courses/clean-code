namespace Markdown
{
    public class PairedTag: Tag
    {
        public PairedTag(TagType type, int start, bool isOpening = true) : base(type, start, isOpening)
        {
        }
        
        public virtual bool TryMatchTagPair(PairedTag other, string text)
        {
            if ((IsTagInMiddleOfWord(text) || other.IsTagInMiddleOfWord(text))
                && !IsTagsInSameWord(other, text)) return false;

            if (text.IsSpace(other.Start - 1)) return false;

            if (Start + Length == other.Start) return false;

            other.IsOpening = false;
            return true;
        }
        
        private bool IsTagInMiddleOfWord(string text)
        {
            return text.IsLetter(Start - 1) && text.IsLetter(Start + Length);
        }

        private bool IsTagsInSameWord(Tag second, string text)
        {
            var start = Start + Length;
            var length = second.Start - start;
            return !text.Substring(start, length).Contains(" ");
        }
    }
}