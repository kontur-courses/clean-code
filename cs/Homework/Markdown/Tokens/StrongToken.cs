namespace Markdown.Tokens
{
    public class StrongToken : MarkdownToken
    {
        public StrongToken(string value, string selector, int paragraphIndex, int startIndex) : base(value, selector, paragraphIndex, startIndex)
        {
        }

        public override string OpenHtmlTag => "<strong>";
        public override string CloseHtmlTag => "</strong>";

        public override string GetHtmlFormatted()
        {
            var valueWithoutSelectors = Value.Trim('_');
            return $"{OpenHtmlTag}{valueWithoutSelectors}{CloseHtmlTag}";
        }
    }
}