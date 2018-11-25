namespace Markdown
{
    public class HtmlTag
    {
        public HtmlTag(string htmlTemplate, int position)
        {
            HtmlTemplate = htmlTemplate;
            Position = position;
        }

        public string HtmlTemplate { get; }
        public int Position { get; }
    }
}
