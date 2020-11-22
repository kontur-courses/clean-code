namespace Markdown
{
    public class UnorderedListTagHelper : TagHelper
    {
        public UnorderedListTagHelper()
            : base("", "<ul>")
        {
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false)
        {
            tag = null;
            return false;
        }

        public override bool ParseForEscapeTag(int position, string text)
        {
            return false;
        }

        public static Tag GetTag(int position, bool isOpening)
        {
            return new Tag(position, TagType.UnorderedList, isOpening, 0, false, false);
        }
    }
}