namespace Markdown.Inline
{
    public class LinkTag : InlineTag
    {
        public string Text { get; }
        public string Url { get; }
        public LinkTag(string text, string url)
        {
            Text = text;
            Url = url;
        }
    }
}
