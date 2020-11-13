namespace Markdown
{
    public class BoldTag : WordTag
    {
        private BoldTag(int position)
            : base("__", "<strong>", position, true, true)
        {
        }


        public override bool TryParse(int position, string text, out Tag tag)
        {
            if (IsTag(position, text))
            {
                tag = new BoldTag(position);
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
            return new BoldTag(0);
        }
    }
}