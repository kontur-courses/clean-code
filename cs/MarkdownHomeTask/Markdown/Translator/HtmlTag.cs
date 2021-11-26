namespace Markdown
{
    public class HtmlTag
    {
        public readonly string Value;

        public HtmlTag(string value)
        {
            Value = value;
        }

        public string Opened => $"<{Value}>";
        public string Closing => $"</{Value}>";
    }
}