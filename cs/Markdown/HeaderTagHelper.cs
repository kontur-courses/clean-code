namespace Markdown
{
    public class HeaderTagHelper : LineTagHelper
    {
        private HeaderTagHelper()
            : base("# ", "<h1>", TagType.Header)
        {
        }

        public static Tag GetCloseTag(int position)
        {
            return new Tag(position, TagType.Header, false, 0, false, false);
        }

        public static TagHelper CreateInstance()
        {
            return new HeaderTagHelper();
        }
    }
}