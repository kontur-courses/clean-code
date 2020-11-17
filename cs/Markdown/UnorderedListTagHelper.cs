namespace Markdown
{
    public class UnorderedListTagHelper : TagHelper
    {
        private UnorderedListTagHelper() 
            : base("", "<ul>")
        {
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false, int lineNumber = 0)
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

        public static TagHelper CreateInstance()
        {
            return new UnorderedListTagHelper();
        }
    }
}