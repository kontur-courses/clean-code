namespace Markdown.Tags
{
    public class Tag
    {
        public string Value { get; }
        public string HtmlValue { get; }
        public string CompletionSign { get; }

        public Tag(string value, string htmlValue, string completionSign)
        {
            Value = value;
            HtmlValue = htmlValue;
            CompletionSign = completionSign;
        }
    }
}