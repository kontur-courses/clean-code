namespace Markdown
{
    public class ItalicTag : WordTag
    {
        private ItalicTag(int position)
            : base("_", "<em>", position, true, true)
        {
        }

        public override bool TryParse(int position, string text, out Tag tag)
        {
            if (IsTag(position, text) && (position + 1 == text.Length || text[position + 1] != '_'))
            {
                tag = new ItalicTag(position);
                return true;
            }

            tag = null;
            return false;
        }

        public override int GetMdTagLengthToSkip()
        {
            return MdTag.Length;
        }

        public static Tag CreateInstance()
        {
            return new ItalicTag(0);
        }
    }
}