namespace Markdown
{
    public class HeaderTokenConverter : TagTokenConverter
    {
        public HeaderTokenConverter()
        {
            OpenTag = "<h1>";
            CloseTag = "</h1>";
        }
    }
}