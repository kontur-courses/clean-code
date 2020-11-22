namespace Markdown
{
    public class BoldTagHelper : TagHelper
    {
        public BoldTagHelper()
            : base("__", "<strong>")
        {
        }


        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false)
        {
            if (IsTag(position, text))
            {
                tag = new Tag(position, TagType.Bold, true, MdTag.Length, inWord, true);
                return true;
            }

            tag = null;
            return false;
        }
    }
}