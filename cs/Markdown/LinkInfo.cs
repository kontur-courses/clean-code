namespace Markdown
{
    public class LinkInfo
    {
        public readonly string Text;
        public readonly string Source;

        public LinkInfo(string text, string source)
        {
            Text = text;
            Source = source;
        }
    }
}