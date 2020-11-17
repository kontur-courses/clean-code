namespace Markdown
{
    public class HeaderTagHelper : LineTagHelper
    {
        private HeaderTagHelper()
            : base("# ", "<h1>", TagType.Header)
        {
        }

        public static TagHelper CreateInstance()
        {
            return new HeaderTagHelper();
        }
    }
}