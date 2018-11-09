namespace Markdown
{
    public class SingleUnderscore : ISpanElement
    {
        private string indicator = "_";

        public string GetOpeningIndicator()
        {
            return indicator;
        }

        public string GetClosingIndicator()
        {
            return indicator;
        }

        public string ToHtml(string markdown)
        {
            return $"<em>{markdown}</em>";
        }

        public bool Contains(ISpanElement spanElement)
        {
            return false;
        }
    }
}
