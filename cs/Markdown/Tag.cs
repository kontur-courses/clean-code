namespace Markdown
{
    public abstract class Tag
    {
        private readonly string htmlTag;

        protected bool IsOpening;

        protected Tag(string mdTag, string htmlTag, int position, bool isOpening)
        {
            MdTag = mdTag;
            this.htmlTag = htmlTag;
            Position = position;
            IsOpening = isOpening;
        }

        public string MdTag { get; }
        public int Position { get; }


        public abstract bool TryParse(int position, string text, out Tag tag);

        public string GetHtmlTag()
        {
            return IsOpening ? htmlTag : htmlTag.Insert(1, "/");
        }

        public abstract int GetMdTagLengthToSkip();

        protected bool IsTag(int position, string text)
        {
            return position + MdTag.Length <= text.Length
                   && text.Substring(position, MdTag.Length) == MdTag;
        }
    }
}