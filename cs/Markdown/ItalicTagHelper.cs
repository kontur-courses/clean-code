namespace Markdown
{
    public class ItalicTagHelper : TagHelper
    {
        public ItalicTagHelper()
            : base("_", "<em>")
        {
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false)
        {
            if (IsTag(position, text) && (position + 1 == text.Length || text[position + 1] != '_'))
            {
                tag = new Tag(position, TagType.Italic, true, MdTag.Length, inWord, true);
                return true;
            }

            tag = null;
            return false;
        }
    }
}