namespace Markdown
{
    public class ListItemTagHelper : LineTagHelper
    {
        private ListItemTagHelper()
            : base("* ", "<li>", TagType.ListItem)
        {
        }

        public static TagHelper CreateInstance()
        {
            return new ListItemTagHelper();
        }
    }
}