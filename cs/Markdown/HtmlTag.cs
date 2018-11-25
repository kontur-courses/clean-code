namespace Markdown
{
    public class HtmlTag
    {
        public HtmlTag(string htmlTemplate, int position, LocationType type)
        {
            HtmlTemplate = htmlTemplate;
            Position = position;
            Type = type;
        }

        public string HtmlTemplate { get; }
        public int Position { get; }

        public LocationType Type { get; }
    }
}
