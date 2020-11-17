namespace Markdown
{
    public class ListItemTagHelper : LineTagHelper
    {
        private ListItemTagHelper()
            : base("* ", "<li>", TagType.ListItem)
        {
        }

        public static Tag GetCloseTag(int position)
        {
            return new Tag(position, TagType.ListItem, false, 0, false, false);
        }

        public static TagHelper CreateInstance()
        {
            return new ListItemTagHelper();
        }
    }
}