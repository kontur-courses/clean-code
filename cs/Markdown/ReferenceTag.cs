namespace Markdown
{
    public class ReferenceTag : PairedTag
    {
        private PairedTag split;
        private PairedTag end;
        private string link;
        private string linkName;
        
        public ReferenceTag(int start) : base(TagType.Reference, start)
        {
        }

        public override bool TryMatchTagPair(PairedTag other, string text)
        {
            if (split == null)
            {
                if (text[other.Start] == ']' && text[other.Start + 1] == '(') split = other;
            }
            else if (text[other.Start] == ')')
            {
                end = other;
                Length = end.Start  - Start;
                linkName = text.Substring(Start + 1, split.Start - Start - 1);
                link = text.Substring(split.Start + 2, end.Start - split.Start - 2);
                return true;
            }

            return false;
        }

        public override string ToHtml()
        {
            if (split == null || end == null) return "";
            return $"<a href=\"{link}\">{linkName}</a>".Replace("\\", "");
        }
    }
}