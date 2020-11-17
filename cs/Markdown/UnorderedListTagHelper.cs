namespace Markdown
{
    public class UnorderedListTagHelper : TagHelper
    {
        private UnorderedListTagHelper() 
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

        public static TagHelper CreateInstance()
        {
            return new UnorderedListTagHelper();
        }
    }
}