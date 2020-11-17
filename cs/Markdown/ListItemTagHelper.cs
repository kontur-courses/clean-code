namespace Markdown
{
    public class ListItemTagHelper : TagHelper
    {
        private ListItemTagHelper() 
            : base("* ", "<li>")
        {
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false, int lineNumber = 0)
        {
            if (IsTag(position, text) && IsAfterNewLine(position, text))
            {
                tag = new Tag(position, TagType.ListItem, true, MdTag.Length, inWord, false, lineNumber);
                return true;
            }

            tag = null;
            return false;
        }
        
        public override bool ParseForEscapeTag(int position, string text)
        {
            return IsTag(position + 1, text) && IsAfterNewLine(position, text);
        }
        
        public static TagHelper CreateInstance()
        {
            return new ListItemTagHelper();
        }
    }
}