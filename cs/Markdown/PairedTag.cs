namespace Markdown
{
    public class PairedTag: Tag
    {
        public bool IsOpening = true;
        
        public PairedTag(TagType type, int start) : base(type, start)
        {
        }
        
        public override string ToHtml()
        {
            var html = TagInfos[Type].Html;
            return IsOpening ? html : html.Insert(1, "/");
        }
        
        public bool IsCorrectTagPair(PairedTag other, string text)
        {
            if ((IsTagInMiddleOfWord(text) || other.IsTagInMiddleOfWord(text))
                && !IsTagsInSameWord(other, text)) return false;

            if (text.IsSpace(other.Start - 1)) return false;

            if (Start + Length == other.Start) return false;
            
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