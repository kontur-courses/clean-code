namespace Markdown.Tokens
{
    public class HeaderToken: MarkdownToken
    {
        public HeaderToken(string value, string selector, int paragraphIndex, int startIndex) : base(value, selector, paragraphIndex, startIndex)
        {
        }
        
        public override string OpenHtmlTag => $"<h{Selector.Length - 1}>";
        public override string CloseHtmlTag => $"</h{Selector.Length - 1}>";

        public override string GetHtmlFormatted()
        {
            var valueWithoutSelectors = Value[(Selector.Length)..];
            return $"{OpenHtmlTag}{valueWithoutSelectors}{CloseHtmlTag}";
        }
    }
}